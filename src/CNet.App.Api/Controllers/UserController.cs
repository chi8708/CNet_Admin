using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CNet.App.Api.Models;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CNet.App.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly WeChatConfig _weChatConfig;
    private readonly ILogger<UserController> _logger;
    private const string WxApiHost = "https://api.weixin.qq.com";

    public UserController(IOptions<WeChatConfig> weChatConfig, ILogger<UserController> logger)
    {
        _weChatConfig = weChatConfig.Value;
        _logger = logger;
    }

    [HttpPost("OnLogin")]
    public async Task<IActionResult> OnLogin([FromBody] LoginRequest request)
    {
        try
        {
            var response = await ExecuteSnsJsCode2SessionAsync(request.Code);
            
            if (response.ErrCode != 0)
            {
                return BadRequest(new { message = response.ErrMsg });
            }

            return Ok(new { 
                openId = response.OpenId,
                sessionKey = response.SessionKey
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "微信登录失败");
            return StatusCode(500, new { message = "登录处理失败" });
        }
    }

    [HttpPost("DecryptPhoneNumber")]
    public async Task<IActionResult> DecryptPhoneNumber([FromBody] PhoneNumberRequest request)
    {
        try
        {
            var wxLoginResponse = await ExecuteSnsJsCode2SessionAsync(request.Code);
            
            if (wxLoginResponse.ErrCode != 0)
            {
                return BadRequest(new { message = wxLoginResponse.ErrMsg });
            }

            var phoneInfo = DecryptData(request.EncryptedData, request.Iv, wxLoginResponse.SessionKey);
            if (phoneInfo == null)
            {
                return BadRequest(new { message = "手机号解密失败" });
            }

            return Ok(phoneInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "解密手机号失败");
            return StatusCode(500, new { message = "手机号处理失败" });
        }
    }

    private async Task<WeChatLoginResponse> ExecuteSnsJsCode2SessionAsync(string code)
    {
        var url = $"{WxApiHost}/sns/jscode2session";
        
        var response = await url
            .SetQueryParams(new
            {
                appid = _weChatConfig.AppId,
                secret = _weChatConfig.AppSecret,
                js_code = code,
                grant_type = "authorization_code"
            })
            .GetJsonAsync<WeChatLoginResponse>();

        return response;
    }

    private object? DecryptData(string encryptedData, string iv, string sessionKey)
    {
        try
        {
            byte[] keyBytes = Convert.FromBase64String(sessionKey);
            byte[] ivBytes = Convert.FromBase64String(iv);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(encryptedBytes))
                using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    string result = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<object>(result);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "解密数据失败");
            return null;
        }
    }
} 