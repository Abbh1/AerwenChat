using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 登录用户信息存储
    /// </summary>
    public class ChatUserDto
    {
        public int ChatUserId { get; set; }
        public long ChatUserGuid { get; set; }
        public string ChatUserNickName { get; set; }
        public string ChatUserName { get; set; }
        public string ChatUserImg { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class ChatUserQueryDto : PagerInfo
    {
        public string ChatUserName { get; set; }
        public string ChatUserNickName { get; set; }
        public int? ChatUserStatus { get; set; }
    }

}
