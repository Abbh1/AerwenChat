using System;
using System.Collections.Generic;
using System.Text;

namespace ARW.Admin.WebApi.Hubs
{
    /// <summary>
    /// 在线用户（聊天室）
    /// </summary>
    public class OnlineUsersForChat
    {
        /// <summary>
        /// 客户端连接Id
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long? UserGuid { get; set; }
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }

        public OnlineUsersForChat(string clientid, string name, long? userguid)
        {
            ConnectionId = clientid;
            Name = name;
            LoginTime = DateTime.Now;
            UserGuid = userguid;
        }
    }
}
