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
using OAuth.Domain.Aggregates;
using OAuth.Domain.Enums;
using OneForAll.Core.OAuth;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core.Extension;
using System.Runtime.CompilerServices;
using OAuth.Repository;

namespace OAuth.Domain
{
    /// <summary>
    /// 领域服务：登录
    /// </summary>
    public class OAuthLoginManager : BaseManager, IOAuthLoginManager
    {
        private readonly IMapper _mapper;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysTenantRepository _tenantRepository;
        private readonly OAuthLoginSettingVo _setting;
        public OAuthLoginManager(
            IMapper mapper,
            ISysUserRepository userRepository,
            ISysTenantRepository tenantRepository,
            OAuthLoginSettingVo setting)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _setting = setting;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUser">登录信息</param>
        /// <returns>结果</returns>
        public async Task<OAuthLoginResultVo> LoginAsync(OAuthLoginVo loginUser)
        {
            var result = new OAuthLoginResultVo() { ErrType = BaseErrType.Success };
            var user = await _userRepository.GetAsync(loginUser.UserName);
            ValidateUserAsync(user, result);
            if (result.ErrType == BaseErrType.Success)
            {
                ValidateTenantAsync(user, result);
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
            }
            return result;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="result">传入结果</param>
        /// <returns>结果</returns>
        private void ValidateUserAsync(SysUser user, OAuthLoginResultVo result)
        {
            if (user == null)
            {
                // 账号不存在
                result.ErrType = BaseErrType.DataNotFound;
            }
            else if (user.Status == SysUserStatusEnum.Normal)
            {
                var loginUser = _mapper.Map<SysUser, LoginUser>(user);
                result.User = loginUser;
            }
            else if (user.Status == SysUserStatusEnum.Frozen)
            {
                // 账号冻结
                result.ErrType = BaseErrType.Frozen;
                ValidateBanTimeAsync(user, result);
            }
            else if (user.Status == SysUserStatusEnum.Banned)
            {
                // 永久封禁
                result.ErrType = BaseErrType.NotAllow;
            }
            else
            {
                // 账号异常
                result.ErrType = BaseErrType.DataError;
            }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="result">传入结果</param>
        /// <returns>结果</returns>
        private async void ValidateTenantAsync(SysUser user, OAuthLoginResultVo result)
        {
            var tenant = await _tenantRepository.FindAsync(user.SysTenantId);
            if (tenant == null)
            {
                // 新注册用户不一定有租户，为不阻止用户注册登录流程，此处返回success
                result.ErrType = BaseErrType.Success;
            }
            else
            {
                result.SysTenant = tenant;
                if (!tenant.IsEnabled)
                    result.ErrType = BaseErrType.PermissionNotEnough;
            }
        }

        /// <summary>
        /// 验证封禁时间
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="result">传入结果</param>
        private void ValidateBanTimeAsync(SysUser user, OAuthLoginResultVo result)
        {
            result.LessBanTime = 0;
            var bantime = GetBanTotalMinutes(user);
            if (bantime > 0)
            {
                var timespan = DateTime.Now - (user.UpdateTime == null ? DateTime.Now : user.UpdateTime.Value);
                result.LessBanTime = bantime - timespan.TotalMinutes;
                if (timespan.TotalMinutes <= bantime)
                {
                    result.ErrType = BaseErrType.Frozen;
                }
            }
        }

        // 获取禁止登录时间
        private int GetBanTotalMinutes(SysUser user)
        {
            switch (user.Status)
            {
                case SysUserStatusEnum.Normal: return 0;
                case SysUserStatusEnum.Frozen: return 30;
                case SysUserStatusEnum.FrozenOneHour: return 60;
                case SysUserStatusEnum.FrozenThreeHour: return 180;
                case SysUserStatusEnum.FrozenDay: return 1440;
                case SysUserStatusEnum.FrozenThreeDay: return 4320;
                case SysUserStatusEnum.FrozenSevenDay: return 10080;
                case SysUserStatusEnum.FrozenMonth: return 43200;
                case SysUserStatusEnum.FrozenThreeMonth: return 129600;
                default: return int.MaxValue;
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="loginUser">登录用户信息</param>
        /// <param name="result">结果</param>
        private async Task ValidatePasswordAsync(SysUser user, OAuthLoginVo loginUser, OAuthLoginResultVo result)
        {
            var pass = user.Password.Equals(loginUser.Password);
            if (!pass)
            {
                user.PwdErrCount += 1;
                if (user.PwdErrCount >= _setting.MaxPwdErrCount)
                {
                    user.Status = SysUserStatusEnum.Frozen;

                    // 错误超过最大次数后需要验证码
                    result.IsRequiredCaptcha = true;
                    result.LessPwdErrCount = 0;
                    result.ErrType = BaseErrType.Frozen;
                    result.LessBanTime = _setting.BanTime;
                }
                else
                {
                    result.LessPwdErrCount = _setting.MaxPwdErrCount - user.PwdErrCount;
                    result.ErrType = BaseErrType.PasswordInvalid;
                }
                user.UpdateTime = DateTime.Now;
                var effected = await _userRepository.SaveChangesAsync();
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
            user.Status = SysUserStatusEnum.Normal;
            return await ResultAsync(() => _userRepository.SaveChangesAsync());
        }
    }
}
