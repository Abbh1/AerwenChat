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
    /// 聊天记录Api
    /// </summary>
    [Route("business/[controller]")]
    public class ChatLogController : BaseController
    {
        private readonly IChatLogService _ChatLogService;

        public ChatLogController(IChatLogService chatLogService)
        {
            _ChatLogService = chatLogService;
        }

        /// <summary>
        /// 获取最近聊天列表
        /// </summary>
        [HttpPost("chatLogList")]
        public IActionResult ChatLogList([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.GetChatLogList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 好友聊天记录列表
        /// </summary>
        [HttpPost("chatFriendLogList")]
        public IActionResult GetChatFriendLogList([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.GetChatFriendLogList(parm);
            return SUCCESS(res);
        }

        ///<summary>
        /// 群聊聊天列表
        ///</summary>
        [HttpPost("chatGroupLogList")]
        public IActionResult ChatGroupLogList([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.GetChatGroupLogList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 好友聊天历史记录列表
        /// </summary>
        [HttpPost("chatFriendLogHistoryList")]
        public IActionResult GetChatFriendHistoryLogList([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.GetChatFriendHistoryLogList(parm);
            return SUCCESS(res);
        }

        ///<summary>
        /// 群聊聊天历史列表
        ///</summary>
        [HttpPost("chatGroupLogHistoryList")]
        public IActionResult ChatGroupLogHistoryList([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.GetChatGroupLogHistoryList(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 获取自己发的消息(最新)
        /// </summary>
        [HttpPost("slefNewMsg")]
        public IActionResult SelfNewMsg([FromBody] ChatLogQueryDto parm)
        {
            var res = _ChatLogService.SelfNewMsg(parm);
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加好友聊天记录
        /// </summary>
        [HttpPost("addChatFriendLog")]
        public IActionResult AddChatFriendLog([FromBody] ChatLogDto parm)
        {
            var res = _ChatLogService.AddChatFriendLog(parm);
            return SUCCESS("发送成功！");
        }

        /// <summary>
        ///已读处理
        /// </summary>
        [HttpPost("read")]
        public IActionResult Read([FromBody] ChatLogQueryDto parm)
        {
            _ChatLogService.Read(parm);
            return SUCCESS("已读");
        }

        /// <summary>
        /// 群聊已读处理
        /// </summary>
        [HttpPost("groupRead")]
        public IActionResult GroupRead([FromBody] ChatLogQueryDto parm)
        {
            _ChatLogService.GroupRead(parm);
            return SUCCESS("已读");
        }

    }
}
