using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Models.Business
{
    [SugarTable("tb_class")]
    public class Class : BusinessBase
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "class_id")]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [SugarColumn(ColumnDescription = "班级名称",ColumnName = "class_name")]
        public string ClassName { get; set; }

    }
}
