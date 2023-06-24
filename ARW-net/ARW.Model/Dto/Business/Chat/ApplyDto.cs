using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARW.Model.Chat
{
    /// <summary>
    /// 申请
    /// </summary>
    public class ApplyDto
    {
        public long SenderGuId { get; set; }
        public long ReceiverGuId { get; set; }
        public string Postscript { get; set; }
        public string Reply { get; set; }
        public bool IsGroup { get; set; }
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class ApplyQueryDto : PagerInfo
    {
        public long? UserGuId { get; set; }
    }
}
