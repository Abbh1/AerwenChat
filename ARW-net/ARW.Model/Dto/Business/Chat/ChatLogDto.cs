using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 聊天记录
    /// </summary>
    public class ChatLogDto
    {
        public string ChatLogType { get; set; }
        public long SenderGuId { get; set; }
        public long ReceiverGuId { get; set; }
        public string ChatLogContent { get; set; }
    }

    public class ChatLogQueryDto : PagerInfo
    {
        public string ChatLogType { get; set; }
        public long? UserGuId { get; set; }
        public long? FriendsGuId { get; set; }
        public string ChatLogContent { get; set; }
    }
}
