using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;
using ARW.Model.Models.Business.Crawler;
using Newtonsoft.Json.Linq;
using ARW.Model.Models.Business.Chat;

namespace ARW.Model.Vo.Chat
{
    public class GroupVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long GroupGuId { get; set; }

        public string GroupName { get; set; }

        public string GroupImg { get; set; }

        public int GroupNum { get; set; }

        public ChatUser GroupUserList { get; set; }

        public DateTime CreateTime { get; set; }

    }

    public class GroupListVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long GroupGuId { get; set; }

        public string GroupName { get; set; }

        public string GroupImg { get; set; }

        public bool GroupIsRead { get; set; }
    }
}
