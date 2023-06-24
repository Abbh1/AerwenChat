using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Dto.Business.Product;
using ARW.Model.Models.Business.Product;
using ARW.Model.Vo;
using ARW.Repository.Business;

namespace ARW.Service.Business.IBusinessService
{
    public interface IProductTypeService : IBaseService<ProductType>
    {

        List<ProductTypeVo> GetTreeList(ProductTypeQueryDto parm);
    }
}
