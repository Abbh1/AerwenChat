using Aliyun.OSS;
using ARW.Admin.WebApi.Extensions;
using ARW.Model.Chat;
using ARW.Model.Models.Business;
using ARW.Model.Models.Business.Chat;
using ARW.Service.Business.IBusinessService;
using ARW.Service.Business.IBusinessService.Chat;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace ARW.Admin.WebApi.Controllers.Business.Api.Chat
{
    /// <summary>
    /// 申请
    /// </summary>
    [Route("business/[controller]")]
    public class ApplyController : BaseController
    {
        private readonly IApplyService _ApplyService;
        private readonly IChatLoginService _ChatLoginService;
        private readonly IFriendsService _FriendsService;
        private readonly IGroupUserService _GroupUserService;
        private readonly IGroupService _GroupService;

        public ApplyController(
            IApplyService ApplyService, IFriendsService friendsService, IGroupUserService groupUserService, IGroupService groupService)
        {
            this._ApplyService = ApplyService;
            _FriendsService = friendsService;
            _GroupUserService = groupUserService;
            _GroupService = groupService;
        }

        /// <summary>
        /// 发送申请
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendApply")]
        public IActionResult SendApply([FromBody] ApplyDto dto)
        {
            if (dto == null) { throw new CustomException("请求参数错误"); }
            var apply = _ApplyService.GetFirst(s => s.SenderGuId == dto.SenderGuId && s.ReceiverGuId == dto.ReceiverGuId && s.IsAgree == null);
            if (apply != null)
            {
                throw new CustomException("已发送申请，请勿重复发送！");
            }


            if (dto.IsGroup)
            {
                var groupuser = _GroupUserService.GetFirst(s => s.GroupGuId == dto.ReceiverGuId && s.UserGuId == dto.SenderGuId);
                if (groupuser != null)
                {
                    throw new CustomException("你已在群中，请勿重复添加！");
                }

                var group = _GroupService.GetFirst(s => s.GroupGuId == dto.ReceiverGuId);
                var groupManager = _GroupUserService.GetFirst(s => s.GroupGuId == group.GroupGuId && s.IsGroupManager == true);

                Apply model = new Apply
                {
                    SenderGuId = dto.SenderGuId,
                    ReceiverGuId = groupManager.UserGuId,
                    Postscript = dto.Postscript,
                    IsGroupApply = true
                };

                var res = _ApplyService.Insertable(model).ExecuteCommandAsync();
                return SUCCESS("已发送群申请");
            }
            else
            {
                var user = _FriendsService.GetFirst(s => s.UserGuId == dto.SenderGuId && s.FriendsGuId == dto.ReceiverGuId);
                if (user != null)
                {
                    throw new CustomException("你们已是好友，请勿重复添加！");
                }
            }

            var modal = dto.Adapt<Apply>().ToCreate(HttpContext);
            var response = _ApplyService.Insertable(modal).ExecuteCommandAsync();

            return SUCCESS("已发送申请");
        }

        /// <summary>
        /// 申请列表
        /// </summary>
        [HttpGet("applyList")]
        public IActionResult GetApplyList([FromQuery] ApplyQueryDto parm)
        {

            var res = _ApplyService.GetApplyList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 查看申请列表
        /// </summary>
        [HttpPost("checkApply")]
        public IActionResult CheckApply([FromQuery] ApplyQueryDto parm)
        {
            var res = _ApplyService.Update(s => s.ReceiverGuId == parm.UserGuId,
                f => new Apply
                {
                    IsRead = true,
                });

            return SUCCESS(res);
        }

        /// <summary>
        ///已读处理
        /// </summary>
        [HttpPost("read")]
        public IActionResult Read([FromBody] ApplyQueryDto parm)
        {
            _ApplyService.Update(s => s.ReceiverGuId == parm.UserGuId,
                f => new Apply
                {
                    IsRead = true
                });
            return SUCCESS("已读");
        }

    }
}
