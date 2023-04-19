using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Models;
using OAuth.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Threading.Tasks;
using OAuth.Domain.Repositorys;
using OAuth.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core.Extension;
using OAuth.Repository;

namespace OAuth.Domain
{
    /// <summary>
    /// 领域服务：登录
    /// </summary>
    public class OAuthLoginManager : BaseManager, IOAuthLoginManager
    {
        private readonly IMapper _mapper;
        private readonly IOAuthUserRepository _userRepository;
        private readonly OAuthLoginSetting _setting;
        public OAuthLoginManager(
            IMapper mapper,
            IOAuthUserRepository userRepository,
            OAuthLoginSetting setting)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _setting = setting;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUser">登录信息</param>
        /// <returns>结果</returns>
        public async Task<OAuthLoginResult> LoginAsync(OAuthLogin loginUser)
        {
            var result = new OAuthLoginResult() { ErrType = BaseErrType.Success };
            var user = await _userRepository.GetWithTenantAsync(loginUser.UserName);
            ValidateUserAsync(user, result);
            if (result.ErrType == BaseErrType.Success)
            {
                ValidateBanTimeAsync(user, result);
                if (result.ErrType == BaseErrType.Success)
                {
                    await ValidatePasswordAsync(user, loginUser, result);
                    if (result.ErrType == BaseErrType.Success)
                    {
                        await LoginSuccessAsync(user, loginUser.IPAddress);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="result">传入结果</param>
        /// <returns>结果</returns>
        private void ValidateUserAsync(SysUser user, OAuthLoginResult result)
        {
            if (user == null || user.SysTenant == null)
            {
                result.ErrType = BaseErrType.DataNotFound;
            }
            else if (user.Status == (int)BaseErrType.NotAllow || user.SysTenant.IsEnabled == false)
            {
                result.ErrType = BaseErrType.NotAllow;
            }
            else
            {
                var loginUser = _mapper.Map<SysUser, OAuthLoginUser>(user);
                result.User = loginUser;
            }
        }

        /// <summary>
        /// 验证封禁时间
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="result">传入结果</param>
        private void ValidateBanTimeAsync(SysUser user, OAuthLoginResult result)
        {
            result.LessBanTime = 0;
            if (user.Status == (int)BaseErrType.Frozen)
            {
                var timespan = DateTime.Now - (user.UpdateTime == null ? DateTime.Now : user.UpdateTime.Value);
                var bantime = _setting.BanTime;
                result.LessBanTime = bantime - timespan.TotalMinutes;
                if (timespan.TotalMinutes <= bantime)
                {
                    result.ErrType = BaseErrType.Frozen;
                }
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginUser">登录用户信息</param>
        /// <param name="result">结果</param>
        private async Task ValidatePasswordAsync(SysUser user, OAuthLogin loginUser, OAuthLoginResult result)
        {
            var pass = user.Password.Equals(loginUser.Password);
            if (!pass)
            {
                if (user.PwdErrCount >= _setting.MaxPwdErrCount - 1)
                {
                    user.PwdErrCount = 0;
                    user.UpdateTime = DateTime.Now;
                    user.Status = (int)BaseErrType.Frozen;
                    await _userRepository.UpdateAsync(user);

                    result.LessPwdErrCount = 0;
                    result.ErrType = BaseErrType.Frozen;
                    result.LessBanTime = _setting.BanTime;
                }
                else
                {
                    user.PwdErrCount++;
                    await _userRepository.UpdateAsync(user);
                    result.LessPwdErrCount = _setting.MaxPwdErrCount - user.PwdErrCount;
                    result.ErrType = BaseErrType.PasswordInvalid;
                }
            }
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginIp">登录io</param>
        /// <returns>结果</returns>
        private async Task<BaseErrType> LoginSuccessAsync(SysUser user, string loginIp)
        {
            user.PwdErrCount = 0;
            user.LastLoginIp = loginIp;
            user.LastLoginTime = DateTime.Now;
            user.UpdateTime = DateTime.Now;
            user.Status = (int)BaseErrType.Success;
            return await ResultAsync(() => _userRepository.UpdateAsync(user));
        }
    }
}
