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
using ARW.Service.Business.IBusinessService.Chat;
using ARW.Model.Chat;
using ARW.Model.Models.Business.Chat;
using Infrastructure.Extensions;

namespace ARW.Admin.WebApi.Controllers.Business.ChatUsers
{
    /// <summary>
    /// 班级控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class ChatUserController : BaseController
    {
        private readonly IChatLoginService _ChatLoginService;

        public ChatUserController(IChatLoginService classService)
        {
            this._ChatLoginService = classService;
        }

        /// <summary>
        /// 获取聊天用户列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        [HttpGet("getChatUserList")]
        [ActionPermissionFilter(Permission = "business:chatUser:list")]
        public IActionResult GetChatUserList([FromQuery] ChatUserQueryDto parm)
        {
            var res = _ChatLoginService.GetChatUserList(parm);
            //res.Result = res.Result.Select(a => new ChatUser{ ChatUserGuid = a.ChatUserGuid,Name = a.Name}).ToList();
            return SUCCESS(res);
        }

        /// <summary>
        /// 修改登录状态
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateChatUserStatus")]
        [ActionPermissionFilter(Permission = "business:cahtUser:updateStatus")]
        [Log(Title = "聊天室用户修改登录状态", BusinessType = BusinessType.UPDATE)]
        public IActionResult Update([FromBody] ChatUserDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<ChatUser>().ToUpdate(HttpContext);
        

            var id = modal.ChatUserId.ParseToLong();

            var response = _ChatLoginService.Update(it => it.ChatUserId == id,
                f => new ChatUser
                {
                    Status = modal.Status,
                });

            if(response != 1)
            {
                throw new CustomException("修改失败");
            }

            return SUCCESS("修改成功");
        }

    }
}
