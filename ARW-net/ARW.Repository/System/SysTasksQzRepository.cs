using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.System;

namespace ARW.Repository.System
{
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class SysTasksQzRepository: BaseRepository<SysTasksQz>
    {
    }
}
