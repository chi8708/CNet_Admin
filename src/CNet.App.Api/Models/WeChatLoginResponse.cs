namespace CNet.App.Api.Models;

public class WeChatLoginResponse
{
    public string OpenId { get; set; } = string.Empty;
    public string SessionKey { get; set; } = string.Empty;
    public string UnionId { get; set; } = string.Empty;
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; } = string.Empty;
} 