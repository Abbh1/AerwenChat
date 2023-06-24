using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;
using ARW.Model.Models.Business.Chat;
using ARW.Model.System.Dto;
using ARW.Model.System;
using ARW.Model.Chat;

namespace ARW.Repository.Business.Chat
{
    /// <summary>
    /// 仓储服务
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class FriendsRepository : BaseRepository<Friends>
    {
        /*
         * 复杂的业务逻辑代码
         */
    }
}
