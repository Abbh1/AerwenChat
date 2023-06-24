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
    /// 群聊控制器
    /// </summary>
    [Route("business/[controller]")]
    public class GroupController : BaseController
    {
        private readonly IChatLoginService _ChatLoginService;
        private readonly IGroupService _GroupService;
        private readonly IGroupUserService _GroupUesrService;
        private readonly IFriendsService _FriendsService;
        private readonly IApplyService _ApplyService;

        public GroupController(
            IFriendsService friendsService, IChatLoginService ChatLoginService, IApplyService applyService, IGroupService groupService, IGroupUserService groupUesrService)
        {
            _FriendsService = friendsService;
            _ChatLoginService = ChatLoginService;
            _ApplyService = applyService;
            _GroupService = groupService;
            _GroupUesrService = groupUesrService;
        }

        /// <summary>
        /// 群聊列表
        /// </summary>
        [HttpGet("groupList")]
        public IActionResult GetGroupList([FromQuery] GroupQueryDto parm)
        {
            var res = _GroupService.GetGroupList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 创建群聊
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpPost("createGroup")]
        public IActionResult CreateGroup([FromBody] GroupDto dto)
        {
            if (dto == null) { throw new CustomException("请求参数错误"); }

            var modal = dto.Adapt<Group>().ToCreate(HttpContext);    
            var response = _GroupService.Insertable(modal).ExecuteReturnSnowflakeId();

            var groupUserDto = new GroupUser
            {
                GroupGuId = response,
                UserGuId = dto.UserGuId,
                IsGroupManager = true
            };

            var ress = _GroupUesrService.Insertable(groupUserDto).ExecuteCommandAsync();

            return SUCCESS("创建成功！");
        }

    }
}
