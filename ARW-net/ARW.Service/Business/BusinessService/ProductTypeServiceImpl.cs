using Infrastructure.Attribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Dto.Business.Product;
using ARW.Model.Models.Business.Product;
using ARW.Model.Vo;
using ARW.Repository.Business;
using ARW.Service.Business.IBusinessService;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 产品类型实现类
    /// </summary>
    [AppService(ServiceType = typeof(IProductTypeService), ServiceLifetime = LifeTime.Transient)]
    public class ProductTypeServiceImpl : BaseService<ProductType>, IProductTypeService
    {
        private readonly ProductTypeRepository _ProductTypeRepository;

        public ProductTypeServiceImpl(ProductTypeRepository productTypeRepository)
        {
            _ProductTypeRepository = productTypeRepository;
        }

        /// <summary>
        /// 查询产品类型树列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<ProductTypeVo> GetTreeList(ProductTypeQueryDto parm)
        {
            //开始拼装查询条件
            var predicate = Expressionable.Create<ProductType>();

            //搜索条件查询语法参考Sqlsugar

            var response = _ProductTypeRepository.Queryable()
                .Where(predicate.ToExpression())
                .Select(it => new ProductTypeVo{
                    ProductTypeId = it.ProductTypeId,
                    ParentId = it.ParentId,
                    ProductTypeName = it.ProductTypeName
                 }).ToTree(it => it.Children, it => it.ParentId, 0);
                

            return response;
        }

    }
}
