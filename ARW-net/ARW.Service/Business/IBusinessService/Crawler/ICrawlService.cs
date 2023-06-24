using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Crawler;
using ARW.Model.Models.Business.Crawler;
using ARW.Model.Vo.Crawler;

namespace ARW.Service.Business.IBusinessService.Crawler
{
    public interface ICrawlService : IBaseService<Crawl>
    {

        /// <summary>
        /// 获取不羞涩分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        List<CrawlVo> GetBuxiuseList(CrawlQueryDto parm);


    }
}
