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

namespace ARW.Model.Vo.Chat
{
    public class ChatApplyRequestVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long SenderGuId { get; set; }

        public string SenderName { get; set; }

        public string SenderImg { get; set; }

        public string Postscript { get; set; }

        public string Reply { get; set; }

        public DateTime SendTime { get; set; }

        public bool? IsAgree { get; set; }

        public bool? IsGroupApply { get; set; }

        public bool? IsRead { get; set; }

    }
}
