using ARW.Admin.WebApi.Extensions;
using ARW.Model.Chat;
using ARW.Model.Models.Business.Chat;
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
    /// 登录
    /// </summary>
    [Route("business/[controller]")]
    public class ChatLoginController : BaseController
    {
        //static readonly NLog.Logger logger = NLog.LogManager.GetLogger("ChatLoginController");
        private readonly IChatLoginService chatLoginService;

        public ChatLoginController(
            IChatLoginService chatLoginService)
        {
            this.chatLoginService = chatLoginService;
        }

        private ChatUser Current_user;


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginBody">登录对象</param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        //[Log(Title = "登录")]
        public IActionResult Login([FromBody] ChatUserDto loginBody)
        {
            if (loginBody == null) { throw new CustomException("请求参数错误"); }

            var user = chatLoginService.Login(loginBody);

            return SUCCESS("登录成功！");
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateChatUser")]
        [Log(Title = "聊天用户修改", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateChatUser([FromBody] ChatUserDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<ChatUser>().ToUpdate(HttpContext);

            var data = chatLoginService.GetFirst(it => it.ChatUserId == modal.ChatUserId);
            if (data == null)
            {
                throw new CustomException("用户不存在");
            }


            var response = chatLoginService.Update(it => it.ChatUserId == modal.ChatUserId,
                f => new ChatUser
                {
                    ChatUserNickName = modal.ChatUserNickName,
                    Sex = modal.Sex,
                    Age = modal.Age,
                    ChatUserImg = modal.ChatUserImg,
                    Phone = modal.Phone,
                    Email = modal.Email,
                });

            return SUCCESS(response);
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("getInfo")]
        public IActionResult GetUserInfo([FromBody] ChatUserDto dto)
        {
            if (dto.ChatUserName== null) { throw new CustomException("请求参数错误"); }
            var user = chatLoginService.FindUserByName(dto.ChatUserName);

            return SUCCESS(user);
        }

        /// <summary>
        /// 查看好友登录状态
        /// </summary>
        [HttpGet("checkFriendStatus")]
        public IActionResult CheckFriendStatus([FromQuery] ChatUserDto parm)
        {
            var res = chatLoginService.GetFirst(s => s.ChatUserGuId == parm.ChatUserGuid && s.Status == 1);

            if (res == null) { return SUCCESS("空"); }
            return SUCCESS(res);
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [Log(Title = "注册", BusinessType = Infrastructure.Enums.BusinessType.INSERT)]
        public IActionResult Register([FromBody] ChatUserDto dto)
        {
            if (dto == null) { throw new CustomException("请求参数错误"); }

            var user = chatLoginService.Register(dto);
            if (user == null)
            {
                throw new CustomException("注册失败");
            }

            return SUCCESS(user, "注册成功！");
        }
    }
}
