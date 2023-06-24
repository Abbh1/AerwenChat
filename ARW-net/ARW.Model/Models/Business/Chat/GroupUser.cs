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
    [SugarTable("tb_chat_group_user")]
    public class GroupUser : BusinessBase
    {
        /// <summary>
        /// 群聊id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "group_guid",ColumnDescription = "群guid(外键)")]
        public long GroupGuId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "user_guid", ColumnDescription = "用户guid(外键)")]
        public long UserGuId { get; set; }

        /// <summary>
        /// 是否群主
        /// </summary>
        [SugarColumn(ColumnName = "isgroup_manager", ColumnDescription = "是否为群主")]
        public bool IsGroupManager { get; set; }

        /// <summary>
        /// 是否群主
        /// </summary>
        [SugarColumn(ColumnName = "IsRead", ColumnDescription = "是否已读")]
        public bool IsRead { get; set; }

    }


}
