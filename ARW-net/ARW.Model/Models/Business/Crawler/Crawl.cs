using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Models.Business.Crawler
{
    [SugarTable("crawl")]
    public class Crawl: BusinessBase
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDescription = "名称", IsNullable = true)]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "简介", IsNullable = true)]
        public string Intro { get; set; }

        [SugarColumn(ColumnDescription = "图片", IsNullable = true)]
        public string Cover { get; set; }

        [SugarColumn(ColumnDescription = "链接", IsNullable = true)]
        public string Link { get; set; }

        [SugarColumn(ColumnDescription = "类型", IsNullable = true)]
        public string Type { get; set; }

        [SugarColumn(ColumnDescription = "上传时间", IsNullable = true)]
        public DateTime PublishTime { get; set; }

        [JsonIgnore]
        [SugarColumn(IsIgnore = true)]
        public List<Resource> DownResources { get; set; }

        [SugarColumn(ColumnDescription = "下载资源", IsNullable = true)]
        public string Resources
        {
            get
            {
                if (this.DownResources != null)
                {
                    return JToken.FromObject(this.DownResources).ToString();
                }
                else
                {
                    return "[]";
                }

            }
        }
    }

    public class Resource
    {
        public string Description { get; set; }

        public string Link { get; set; }
    }

}
