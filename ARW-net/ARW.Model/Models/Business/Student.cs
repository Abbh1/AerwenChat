using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Models.Business
{
    [SugarTable("tb_student")]
    public class Student : BusinessBase
    {
        /// <summary>
        /// 学生id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "student_id")]
        public long StudentId { get; set; }

        /// <summary>
        /// 班级guid(外键)
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnDescription = "班级guid(外键)", ColumnName = "class_guid")]
        public long ClassId { get; set; }

        /// <summary>
        /// 字典类型(外键)
        /// </summary>
        [SugarColumn(ColumnDescription = "字典类型(外键)", IsNullable = true, ColumnName = "dictType")]
        public string DictType { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        [SugarColumn(ColumnDescription = "学生名称", IsNullable = true, ColumnName = "student_name")]
        public string StudentName { get; set; }

        /// <summary>
        /// 性别(1:男，2:女)
        /// </summary>
        [SugarColumn(Length = 1, ColumnDescription = "性别(1:男，2:女)", IsNullable = true, ColumnName = "student_sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [SugarColumn(ColumnDescription = "年龄", IsNullable = true, ColumnName = "student_age")]
        public int? Age { get; set; }

        /// <summary>
        /// 学生图片(路径)
        /// </summary>
        [SugarColumn(ColumnDescription = "学生图片", IsNullable = true, ColumnName = "student_img")]
        public string StudentImg { get; set; }

        /// <summary>
        /// 学生标签(单个字段,以","分开)
        /// </summary>
        [SugarColumn(ColumnDescription = "学生标签", IsNullable = true, ColumnName = "student_tag")]
        public string StudentTag { get; set; }

        /// <summary>
        /// 学生服务(json)
        /// </summary>
        [SugarColumn(ColumnDescription = "学生服务", ColumnDataType = "text", IsNullable = true, ColumnName = "student_service", IsJson = true)]
        public Service[] StudentService { get; set; }
        
        /// <summary>
        /// 学生描述(富文本)
        /// </summary>
        [SugarColumn(ColumnDescription = "学生描述", ColumnDataType = "text", IsNullable = true, ColumnName = "student_describe")]
        public string StudentDescribe { get; set; }
    }

    public class Service
    {
        public string service_name { get; set; }
        public string service_price { get; set; }
    }
}
