using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OAuth.Application.Interfaces;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Aggregates;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Application
{
    /// <summary>
    /// 登录
    /// </summary>
    public class OAuthLoginService: IOAuthLoginService
    {
        private readonly OAuthLoginSettingVo _setting;
        private readonly IOAuthLoginManager _loginManager;
        private readonly ICaptchaManager _codeManager;
        private readonly ISysUserRiskRecordManager _riskManager;
        public OAuthLoginService(
            OAuthLoginSettingVo setting,
            IOAuthLoginManager loginManager,
            ICaptchaManager codeManager,
            ISysUserRiskRecordManager riskManager)
        {
            _setting = setting;
            _loginManager = loginManager;
            _codeManager = codeManager;
            _riskManager = riskManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUser">登录信息</param>
        /// <returns>结果</returns>
        public async Task<OAuthLoginResultVo> LoginAsync(OAuthLoginVo loginUser)
        {
            if (loginUser.CaptchaKey == _setting.IgnoreCaptchaKey)
            {
                // 忽略验证码校验
                return await _loginManager.LoginAsync(loginUser);
            }
            else
            {
                if (_setting.IsCaptchaEnabled)
                {
                    // 必须检查验证码
                    var pass = await _codeManager.CheckStrCodeAsync(loginUser.CaptchaKey, loginUser.Captcha);
                    if (!pass)
                        return new OAuthLoginResultVo() { ErrType = BaseErrType.AuthCodeInvalid };
                }
                else
                {
                    // 检查是否存在账号风险监控
                    var risk = await _riskManager.GetAsync(loginUser.UserName);
                    if (risk != null)
                    {
                        var pass = await _codeManager.CheckStrCodeAsync(loginUser.CaptchaKey, loginUser.Captcha);
                        if (!pass)
                            return new OAuthLoginResultVo() { ErrType = BaseErrType.AuthCodeInvalid };
                    }
                }
                var result = await _loginManager.LoginAsync(loginUser);
                if (result.IsRequiredCaptcha)
                {
                    await _riskManager.AddAsync(new SysUserRiskRecord()
                    {
                        Level = 1,
                        UserName = loginUser.UserName,
                        CreateTime = DateTime.Now,
                        Type = SysRiskUserTypeEnum.PwdError,
                        Remark = EnumHelper.GetDescription(SysRiskUserTypeEnum.PwdError),
                    });
                }
                return result;
            }
        }
    }
}
