using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 群聊
    /// </summary>
    public class GroupDto
    {
        public long GroupGuId { get; set; }
        public string GroupName { get; set; }
        public string GroupImg { get; set; }
        public long UserGuId { get; set; }
    }

    public class GroupQueryDto : PagerInfo
    {
        public long? UserGuId { get; set; }
    }

    public class GroupUserDto
    {
        public long GroupManagerGuId { get; set; }
        public long UserGuId { get; set; }
        public string Reply { get; set; }
        public bool IsAgree { get; set; }
    }
}
