using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;
using ARW.Model.Models.Business.Product;

namespace ARW.Repository.Business
{
    /// <summary>
    /// 产品类型仓储服务
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class ProductTypeRepository : BaseRepository<ProductType>
    {
        /*
         * 复杂的业务逻辑代码
         */
    }
}
