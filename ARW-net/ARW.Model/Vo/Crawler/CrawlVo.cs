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

namespace ARW.Model.Vo.Crawler
{
    public class CrawlVo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Intro { get; set; }

        public string Cover { get; set; }

        public string Link { get; set; }

        public string Type { get; set; }

        public DateTime PublishTime { get; set; }

        [JsonIgnore]
        public List<Resource> DownResources { get; set; }

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

}
