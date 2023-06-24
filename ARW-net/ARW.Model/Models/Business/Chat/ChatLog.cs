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
    [SugarTable("tb_chat_log")]
    public class ChatLog : BusinessBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "chat_log_id")]
        public int ChatLogId { get; set; }

        /// <summary>
        /// 聊天记录id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "chat_log_guid")]
        public long ChatLogGuId { get; set; }

        /// <summary>
        /// 聊天记录类型
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "聊天记录类型(1:私聊，2:群聊)", ColumnName = "chat_log_type")]
        public string ChatLogType { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
        /// <summary>
        /// 发送人guid(外键)
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnDescription = "发送人guid(外键)", ColumnName = "sender_guid")]
        public long SenderGuId { get; set; }

        /// <summary>
        /// 接收人guid(外键)
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnDescription = "接收人guid(外键)", ColumnName = "receiver_guid")]
        public long ReceiverGuId { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [SugarColumn(ColumnDescription = "发送时间", ColumnName = "chat_log_send_time")]
        public DateTime ChatLogSendTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(ColumnDescription = "内容", ColumnDataType = "text", ColumnName = "chat_log_content")]
        public string ChatLogContent { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注", IsNullable = true, ColumnName = "chat_log_name")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        [SugarColumn(ColumnDescription = "是否已读", ColumnName = "isRead")]
        public bool IsRead { get; set; }

    }


}
