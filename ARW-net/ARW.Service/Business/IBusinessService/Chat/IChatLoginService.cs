using ARW.Model.Models.Business.Chat;
using ARW.Model.Chat;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Vo.Chat;

namespace ARW.Service.Business.IBusinessService.Chat
{
    public interface IChatLoginService : IBaseService<ChatUser>
    {
        /// <summary>
        /// 获取聊天用户列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ChatUserVo> GetChatUserList(ChatUserQueryDto parm);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        public ChatUser Login(ChatUserDto loginBody);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        public ChatUser Register(ChatUserDto dto);

        public ChatUser FindUserByName(string name);

        public Task<ChatUser> FindUserByGuid(long guid);

    }
}
