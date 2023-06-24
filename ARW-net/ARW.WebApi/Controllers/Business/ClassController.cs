using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Service.Business.IBusinessService;

namespace ARW.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 班级控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ClassController : BaseController
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            this._classService = classService;
        }

        /// <summary>
        /// 获取班级列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getClassList")]
        [ActionPermissionFilter(Permission = "business:class:list")]
        public IActionResult GetClassARWst([FromQuery] ClassQueryDto parm)
        {
            var res = _classService.GetClassList(parm);
            //res.Result = res.Result.Select(a => new Class{ ClassGuid = a.ClassGuid,Name = a.Name}).ToList();
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加班级
        /// </summary>
        /// <returns></returns>
        [HttpPost("addClass")]
        [ActionPermissionFilter(Permission = "business:class:add")]
        [Log(Title = "班级添加", BusinessType = BusinessType.INSERT)]
        public IActionResult Create([FromBody] ClassDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<Class>().ToCreate(HttpContext);

            var response = _classService.Insertable(modal).ExecuteReturnSnowflakeId();
            return SUCCESS(response);
        }

        /// <summary>
        /// 更新班级
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateClass")]
        [ActionPermissionFilter(Permission = "business:class:update")]
        [Log(Title = "班级修改", BusinessType = BusinessType.UPDATE)]
        public IActionResult Update([FromBody] ClassDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<Class>().ToUpdate(HttpContext);

            var response = _classService.Update(it => it.ClassId == modal.ClassId,
                f => new Class
                {
                    ClassName = parm.ClassName,
                });

            return SUCCESS(response);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:class:delete")]
        [Log(Title = "班级删除", BusinessType = BusinessType.DELETE)]
        public IActionResult Delete(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);

            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _classService.Delete(idsArr);

            return ToResponse(response, "删除成功!");
        }

    }
}
