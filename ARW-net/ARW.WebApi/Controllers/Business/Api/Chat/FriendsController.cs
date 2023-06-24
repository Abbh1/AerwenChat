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
    /// 好友
    /// </summary>
    [Route("business/[controller]")]
    public class FriendsController : BaseController
    {
        private readonly IChatLoginService _ChatLoginService;
        private readonly IFriendsService _FriendsService;
        private readonly IGroupService _GroupService;
        private readonly IApplyService _ApplyService;

        public FriendsController(
            IFriendsService friendsService, IChatLoginService ChatLoginService, IApplyService applyService, IGroupService groupService)
        {
            _FriendsService = friendsService;
            _ChatLoginService = ChatLoginService;
            _ApplyService = applyService;
            _GroupService = groupService;
        }

        /// <summary>
        /// 好友列表
        /// </summary>
        [HttpGet("friendsList")]
        public IActionResult GetFriendsList([FromQuery] FriendsQueryDto parm)
        {
            var res = _FriendsService.GetFriendsList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 查找好友
        /// </summary>
        /// <returns></returns>
        [HttpPost("findFriends")]
        public IActionResult FindFriends([FromBody] FindFriendsDto dto)
        {
            if (dto.FriendName == null) { throw new CustomException("请求参数错误"); }
            var user = _ChatLoginService.GetFirst(s => s.ChatUserName ==  dto.FriendName);
            var group = _GroupService.GetFirst(s => s.GroupName == dto.FriendName);

            if(user == null && group == null)
            {
                { throw new CustomException("用户或群聊不存在"); }
            }

            if(user == null && group != null)
            {
                return SUCCESS(group);
            }
            else
            {
                return SUCCESS(user);
            }

        }

        /// <summary>
        /// 修改好友备注
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateNote")]
        [Log(Title = "修改好友备注", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateStudent([FromBody] FriendsDto parm)
        {
            if (parm == null) { throw new CustomException("请求参数错误"); }

            var modal = parm.Adapt<Friends>().ToUpdate(HttpContext);

            var data = _FriendsService.GetFirst(it => it.UserGuId == modal.UserGuId && it.FriendsGuId == modal.FriendsGuId);
            if (data == null)
            {
                throw new CustomException("好友不存在");
            }


            var response = _FriendsService.Update(it => it.UserGuId == modal.UserGuId && it.FriendsGuId == modal.FriendsGuId,
            f => new Friends
            {
                FriendsNote = modal.FriendsNote
            }
            );

            return SUCCESS(response);
        }


        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpPost("addFriend")]
        public async Task<IActionResult> AddFriend([FromBody] FriendsDto dto)
        {
            if (dto == null) { throw new CustomException("请求参数错误"); }

            var modal = dto.Adapt<Friends>().ToCreate(HttpContext);
            var parm = new Apply
            {
                SenderGuId = modal.FriendsGuId,
                ReceiverGuId = modal.UserGuId,
                Reply = dto.Reply,
                IsAgree = dto.IsAgree
            };

            var applyDto = parm.ToUpdate(HttpContext);


            if (dto.IsAgree == true)
            {
                var res = _ApplyService.Update(it => it.ReceiverGuId == applyDto.ReceiverGuId && it.SenderGuId == applyDto.SenderGuId && it.IsGroupApply == null,
                f => new Apply
                {
                    IsAgree = true,
                    Update_time = DateTime.Now
                });


                var Fdto = new Friends
                {
                    UserGuId = modal.FriendsGuId,
                    FriendsGuId = modal.UserGuId,
                    FriendsNote = null
                };
                await _FriendsService.InsertAsync(modal);
                await _FriendsService.InsertAsync(Fdto);

            }
            else
            {
                var res = _ApplyService.Update(it => it.ReceiverGuId == applyDto.ReceiverGuId && it.SenderGuId == applyDto.SenderGuId&& it.IsGroupApply == false,
                f => new Apply
                {
                    IsAgree = false,
                    Reply = applyDto.Reply
                });
            }

            return SUCCESS("添加成功！");
        }


    }
}
