using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 查找好友
    /// </summary>
    public class FindFriendsDto
    {
        public string FriendName { get; set; }
    }

    /// <summary>
    /// 修改备注
    /// </summary>
    public class UpdateNoteDto
    {
        public long UserGuId { get; set; }
        public string Note { get; set; }
    }
}
