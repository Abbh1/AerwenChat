using System;
using Newtonsoft.Json;
using SqlSugar;
using OfficeOpenXml.Attributes;

namespace ARW.Model.Models.Business
{

    [EpplusTable(PrintHeaders = true, AutofitColumns = true, AutoCalculate = true, ShowTotal = true)]
    public class BusinessBase
    {
        
            [SugarColumn(IsOnlyIgnoreUpdate = true,IsNullable = true)]//设置后修改不会有此字段
            [JsonProperty(propertyName: "CreateBy")]
            [EpplusIgnore]
            public string Create_by { get; set; }

            [SugarColumn(IsOnlyIgnoreUpdate = true, IsNullable = true)]//设置后修改不会有此字段
            [JsonProperty(propertyName: "CreateTime")]
            [EpplusTableColumn(NumberFormat = "yyyy-MM-dd HH:mm:ss")]
            public DateTime Create_time { get; set; } = DateTime.Now;

            [JsonIgnore]
            [JsonProperty(propertyName: "UpdateBy")]
            [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true)]
            [EpplusIgnore]
            public string Update_by { get; set; }

            //[JsonIgnore]
            [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true)]//设置后插入数据不会有此字段
            [JsonProperty(propertyName: "UpdateTime")]
            [EpplusIgnore]
            public DateTime? Update_time { get; set; }

            [JsonProperty(propertyName: "IsDelete")]
            [SugarColumn(IsNullable = true)]
            [EpplusIgnore]
            public bool IsDelete { get; set; }
    }
}
