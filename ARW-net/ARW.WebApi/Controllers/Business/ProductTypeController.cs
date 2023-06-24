using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Business.Product;
using ARW.Model.Models.Business.Product;
using ARW.Service.Business.IBusinessService;

namespace ARW.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 产品管理Controller
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ProductTypeController : BaseController
    {

        private readonly IProductTypeService _ProductTypeService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="productTypeService">产品类型服务</param>
        public ProductTypeController(IProductTypeService productTypeService)
        {
            _ProductTypeService = productTypeService;
        }

        /// <summary>
        /// 查询产品类型列表树
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("getProductTypeTreeList")]
        [ActionPermissionFilter(Permission = "business:productType:treeList")]
        public IActionResult ProductTreeList([FromQuery] ProductTypeQueryDto parm)
        {
            var response = _ProductTypeService.GetTreeList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 添加产品类型
        /// </summary>
        /// <returns></returns>
        [HttpPost("addProductType")]
        [ActionPermissionFilter(Permission = "business:productType:add")]
        [Log(Title = "产品类型", BusinessType = BusinessType.INSERT)]
        public IActionResult addProductType([FromBody] ProductTypeDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }
            //从 Dto 映射到 实体
            var modal = parm.Adapt<ProductType>().ToCreate(HttpContext);

            var response = _ProductTypeService.Insertable(modal).ExecuteReturnSnowflakeId();

            return ToResponse(response);
        }

        /// <summary>
        /// 更新产品类型
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateProductType")]
        [ActionPermissionFilter(Permission = "business:productType:edit")]
        [Log(Title = "产品类型", BusinessType = BusinessType.UPDATE)]
        public IActionResult updateProductType([FromBody] ProductTypeDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求实体不能为空");
            }
            var modal = parm.Adapt<ProductType>().ToUpdate(HttpContext);

            var response = _ProductTypeService.Update(it => it.ProductTypeId == modal.ProductTypeId,
             f => new ProductType
             {
                 ProductTypeName = parm.ProductTypeName,
                 ParentId = parm.ParentId,
             });    

            return ToResponse(response);
        }

        /// <summary>
        /// 删除产品类型
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:productType:delete")]
        [Log(Title = "产品类型", BusinessType = BusinessType.DELETE)]
        public IActionResult deleteProductType(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _ProductTypeService.Delete(idsArr);

            return ToResponse(response);
        }


    }
}
