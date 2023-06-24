using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 好友
    /// </summary>
    public class FriendsDto
    {
        public long UserGuId { get; set; }
        public long FriendsGuId { get; set; }
        public string FriendsNote { get; set; }
        public string Reply { get; set; }
        public bool IsAgree { get; set; }
    }

    public class FriendsQueryDto : PagerInfo
    {
        public long? UserGuId { get; set; }
    }
}
