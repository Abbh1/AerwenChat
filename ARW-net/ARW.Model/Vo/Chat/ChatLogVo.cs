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
    public class ChatLogVo
    {
        public string ChatLogType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long SenderGuId { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long ReceiverGuId { get; set; }

        public string ChatLogContent { get; set; }

        public DateTime ChatLogSendTime { get; set; }

        public ChatUser Sender{ get; set; }

    }

    public class ChatLogListVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long SenderGuId { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long ReceiverGuId { get; set; }

        public string ChatLogContent { get; set; }

        public string ChatLogSendTime { get; set; }

        public FriendsVo ChatUserObject
        {
            get
            {
                //var sex = "";
                //if (this.Sex == "1")
                //{
                //    sex = "男";
                //}
                //else
                //{
                //    sex = "女";
                //}

                return new FriendsVo
                {
                    FriendGuId = this.ChatUserGuId,
                    FriendName = this.ChatUserName,
                    FriendNickName = this.ChatUserNickName,
                    FriendImg = this.ChatUserImg,
                    Sex = this.Sex,
                    Age = this.Age,
                    Phone = this.Phone,
                    Email = this.Email,
                    FriendNote = this.FriendNote,
                    IsRead = this.IsRead,
                };
            }
            set { }
        }

        public GroupListVo GroupObject
        {
            get
            {
                return new GroupListVo
                {
                    GroupGuId = this.GroupGuId,
                    GroupName = this.GroupName,
                    GroupImg = this.GroupImg,
                    GroupIsRead = this.GroupIsRead,
                };
            }
            set { }
        }

        public bool IsRead { get; set; }

        [JsonIgnore]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long GroupGuId { get; set; }

        [JsonIgnore]
        public string GroupName { get; set; }

        [JsonIgnore]
        public string GroupImg { get; set; }

        [JsonIgnore]
        public bool GroupIsRead { get; set; }

        [JsonIgnore]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long ChatUserGuId { get; set; }
        [JsonIgnore]
        public string ChatUserName { get; set; }
        [JsonIgnore]
        public string ChatUserNickName { get; set; }
        [JsonIgnore]
        public string ChatUserImg { get; set; }
        [JsonIgnore]
        public string Sex { get; set; }
        [JsonIgnore]
        public int Age { get; set; }
        [JsonIgnore]
        public string Phone { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string FriendNote { get; set; }

    }

}
