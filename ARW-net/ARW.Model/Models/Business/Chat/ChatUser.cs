using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Models.Business.Chat
{
    [SugarTable("tb_chat_user")]
    public class ChatUser : BusinessBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "chat_user_id")]
        public int ChatUserId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "chat_user_guid")]
        public long ChatUserGuId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnDescription = "用户名", ColumnName = "chat_user_name")]
        public string ChatUserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        [EpplusIgnore]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "昵称",  ColumnName = "chat_user_nickname")]
        public string ChatUserNickName { get; set; }

        /// <summary>
        /// 性别(1:男，2:女)
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "性别(1:男，2:女)", IsNullable = true, ColumnName = "chat_user_sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [SugarColumn(ColumnDescription = "年龄", IsNullable = true, ColumnName = "chat_user_age")]
        public int? Age { get; set; }

        /// <summary>
        /// 用户图片(路径)
        /// </summary>
        [SugarColumn(ColumnDescription = "用户头像", IsNullable = true, ColumnName = "chat_user_img")]
        public string ChatUserImg { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [SugarColumn(ColumnDescription = "电话号码", IsNullable = true, ColumnName = "chat_user_phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnDescription = "邮箱", IsNullable = true, ColumnName = "chat_user_email")]
        public string Email { get; set; }

        /// <summary>
        /// 登录状态(1,在线 | 0,离线)
        /// </summary>
        [SugarColumn(ColumnDescription = "登录状态(1,在线 | 0,离线)", ColumnName = "chat_user_status")]
        public int? Status { get; set; }
    }

 
}
