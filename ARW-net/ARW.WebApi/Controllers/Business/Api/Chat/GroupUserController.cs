using Aliyun.OSS;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Model.Chat;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Model.Models.Business.Chat;
using ARW.Service.Business.IBusinessService;
using ARW.Service.Business.IBusinessService.Chat;
using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARW.Admin.WebApi.Controllers.Business.Api.Chat
{
    /// <summary>
    /// 群聊用户
    /// </summary>
    [Route("business/[controller]")]
    public class GroupUserController : BaseController
    {
        private readonly IChatLoginService _ChatLoginService;
        private readonly IFriendsService _FriendsService;
        private readonly IApplyService _ApplyService;
        private readonly IGroupUserService _GroupUserService;

        public GroupUserController(
            IFriendsService friendsService, IChatLoginService ChatLoginService, IApplyService applyService, IGroupUserService groupUserService)
        {
            _FriendsService = friendsService;
            _ChatLoginService = ChatLoginService;
            _ApplyService = applyService;
            _GroupUserService = groupUserService;
        }

        /// <summary>
        /// 添加群聊
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpPost("addGroup")]
        public IActionResult AddGroup([FromBody] GroupUserDto dto)
        {
            if (dto == null) { throw new CustomException("请求参数错误"); }

            var parm = new Apply
            {
                SenderGuId = dto.UserGuId,
                ReceiverGuId = dto.GroupManagerGuId,
                Reply = dto.Reply,
                IsAgree = dto.IsAgree
            };

            var applyDto = parm.ToUpdate(HttpContext);


            if (dto.IsAgree == true)
            {
                var res = _ApplyService.Update(it => it.ReceiverGuId == applyDto.ReceiverGuId && it.SenderGuId == applyDto.SenderGuId && it.IsGroupApply == true,
                f => new Apply
                {
                    IsAgree = true,
                    Update_time = DateTime.Now
                });

                var gourpGuid = _GroupUserService.GetFirst(s => s.UserGuId == dto.GroupManagerGuId && s.IsGroupManager == true).GroupGuId;

                var Gdto = new GroupUser
                {
                    GroupGuId = gourpGuid,
                    UserGuId = dto.UserGuId,
                };
                var ress = _GroupUserService.Insertable(Gdto).ExecuteCommandAsync();

            }
            else
            {
                var res = _ApplyService.Update(it => it.ReceiverGuId == applyDto.ReceiverGuId && it.SenderGuId == applyDto.SenderGuId && it.IsGroupApply == false,
                f => new Apply
                {
                    IsAgree = false,
                    Reply = applyDto.Reply
                });;
            }

            return SUCCESS("添加成功！");
        }


    }
}
