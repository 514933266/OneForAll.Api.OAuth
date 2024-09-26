using AutoMapper;
using OAuth.Domain.AggregateRoots;
using OAuth.Domain.Interfaces;
using OAuth.Domain.Repositorys;
using OAuth.Domain.ValueObjects;
using OAuth.Repository;
using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain
{
    /// <summary>
    /// 用户风险信息
    /// </summary>
    public class SysUserRiskRecordManager : BaseManager, ISysUserRiskRecordManager
    {
        private readonly IMapper _mapper;
        private readonly ISysUserRiskRecordRepository _repository;
        public SysUserRiskRecordManager(
            IMapper mapper,
            ISysUserRiskRecordRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 获取一条风险记录
        /// </summary>
        /// <param name="username">账号</param>
        /// <returns>结果</returns>
        public async Task<SysUserRiskRecord> GetAsync(string username)
        {
            return await _repository.GetAsync(w => w.UserName == username);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysUserRiskRecord entity)
        {
            var data = await _repository.GetAsync(w => w.UserName == entity.UserName);
            if (data != null)
                return BaseErrType.DataExist;
            return await ResultAsync(() => _repository.AddAsync(data));
        }
    }
}
