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
    public class FriendsVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long FriendGuId { get; set; }

        public string FriendName { get; set; }

        public string FriendNickName { get; set; }

        public string FriendImg { get; set; }

        public string FriendNote { get; set; }

        public DateTime CreateTime { get; set; }

        public string Sex { get; set; }

        public int? Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int? Status { get; set; }

        public bool IsRead { get; set; }

    }
}
