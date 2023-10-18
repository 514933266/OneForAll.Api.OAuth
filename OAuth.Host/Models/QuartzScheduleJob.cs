using System;

namespace OAuth.Host.Models
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public class QuartzScheduleJob
    {
        /// <summary>
        /// 定时任务类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public Type JobType { get; set; }

        /// <summary>
        /// Corn表达式
        /// </summary>
        public string Corn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
