using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;

namespace ARW.Model.Dto.Business.Crawler
{

    /// <summary>
    /// 查询对象
    /// </summary>
    public class CrawlQueryDto : PagerInfo
    {

        public string Name { get; set; }
    }

}
