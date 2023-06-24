using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;

namespace ARW.Repository.Business
{
    /// <summary>
    /// 学生仓储服务
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class StudentRepository : BaseRepository<Student>
    {
        /*
         * 复杂的业务逻辑代码
         */
    }
}
