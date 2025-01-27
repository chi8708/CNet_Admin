﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CNet.BLL.Main;
using CNet.Common;
using System.Security.Cryptography;

namespace CNet.Web.Api.Controllers
{
    /// <summary>
    /// 认证
    /// </summary>
    [Route("api/[controller]")]
    //jwt1 身份认证
    public class AuthroizeController:Controller
    {
        private readonly JwtSeetings _jwtSeetings;

        public AuthroizeController(IOptions<JwtSeetings> jwtSeetingsOptions)
        {
            _jwtSeetings = jwtSeetingsOptions.Value;
        }

        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="loginViewModel">登录实体信息</param>
        /// <returns></returns>
        [HttpPost,AllowAnonymous]
        public ActionResult Post([FromBody]LoginViewModel loginViewModel)
        {


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            loginViewModel.Name = QueryHelper.InjectionFilter(loginViewModel.Name);
            loginViewModel.Password = QueryHelper.InjectionFilter(loginViewModel.Password);

            var users = new Pub_UserBLL().GetList($"StopFlag=0 AND UserName='{loginViewModel.Name}' ", limits: 1);
            
            if (users.Count>0)
            {
                var user = users.First();
                if (QueryHelper.StringToMD5Hash(user.UserPwd)!= loginViewModel.Password)
                {
                    return Ok(new ResponseObj<dynamic>()
                    {
                        Code = 0,
                        Message = "用户名密码错误！"
                    });
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim("Id",user.Id.ToString()),
                    new Claim("UserCode",user.UserCode),
                    new Claim("Tel",user.Tel??""),
                    new Claim("DeptCode",user.DeptCode??"")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expires = DateTime.Now.AddMinutes(30);
                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                   expires,
                    creds
                    );

                //string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                //user.Token = tokenStr;
                //CNetFactory.UpdateToken(user);

                return Ok(new ResponseObj<dynamic>()
                {
                    Code = 1,
                    Message = "认证成功",
                    Data = new { Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = TypeUtil.ConvertDateTimeInt(expires) }
                });
            }

            return Ok(new ResponseObj<dynamic>()
            {
                Code = 0,
                Message = "用户名密码错误！"
            });
            //return BadRequest();
        }
    }
}