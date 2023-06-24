﻿using Infrastructure.Attribute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.System;
using ARW.Repository;
using ARW.Service.System.IService;

namespace ARW.Service.System
{
    /// <summary>
    /// 任务日志
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient, ServiceType = typeof(ISysTasksLogService))]
    public class SysTasksLogService : BaseService<SysTasksLog>, ISysTasksLogService
    {
        private ISysTasksQzService _tasksQzService;
        public SysTasksLogService(ISysTasksQzService tasksQzService)
        {
            _tasksQzService = tasksQzService;
        }

        public async Task<SysTasksLog> AddTaskLog(string jobId, SysTasksLog logModel)
        {
            //获取任务信息
            var model = await _tasksQzService.GetSingleAsync(f => f.ID.ToString() == jobId);

            if (model != null)
            {
                logModel.JobId = jobId;
                logModel.JobName = model.Name;
                logModel.JobGroup = model.JobGroup;
                logModel.CreateTime = DateTime.Now;
            }

            await InsertAsync(logModel);
            return logModel;
        }

    }
}
