using Infrastructure.Attribute;
using ARW.Model.System;
using ARW.Repository.System;
using ARW.Service.System.IService;

namespace ARW.Service.System
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [AppService(ServiceType = typeof(ISysTasksQzService), ServiceLifetime = LifeTime.Transient)]
    public class SysTasksQzService : BaseService<SysTasksQz>, ISysTasksQzService
    {
        public SysTasksQzService(SysTasksQzRepository repository)
        {
        }
    }
}
