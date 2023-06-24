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
    [SugarTable("tb_chat_group")]
    public class Group : BusinessBase
    {
        /// <summary>
        /// 群聊id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "group_id")]
        public int GroupId { get; set; }

        /// <summary>
        /// 群聊guid
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "group_guid")]
        public long GroupGuId { get; set; }

        /// <summary>
        /// 群名称
        /// </summary>
        [SugarColumn(ColumnName = "group_name", ColumnDescription = "群名称")]
        public string GroupName { get; set; }

        /// <summary>
        /// 群头像
        /// </summary>
        [SugarColumn(ColumnDescription = "群头像", IsNullable = true, ColumnName = "group_img")]
        public string GroupImg { get; set; }

    }


}
