using Newtonsoft.Json;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;

namespace ARW.Model.Vo
{
    public class StudentVo
    {
        [EpplusTableColumn(Header = "学生Id")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long StudentId { get; set; }

        [EpplusTableColumn(Header = "班级Id")]
        [JsonConverter(typeof(ValueToStringConverter))]
        public long ClassId { get; set; }

        [EpplusTableColumn(Header = "班级名称")]
        public string ClassName { get; set; }

        [EpplusTableColumn(Header = "学生名称")]
        public string StudentName { get; set; }

        [EpplusTableColumn(Header = "学生性别")]
        public string Sex { get; set; }

        [EpplusTableColumn(Header = "学生年龄")]
        public int? Age { get; set; }

        [EpplusTableColumn(Header = "学生图片")]
        public string StudentImg { get; set; }

        [EpplusTableColumn(Header = "学生标签")]
        public string StudentTag { get; set; }

        [EpplusTableColumn(Header = "学生服务")]
        public string StudentService { get; set; }

        [EpplusTableColumn(Header = "学生描述")]
        public string StudentDescribe { get; set; }
    }
}
