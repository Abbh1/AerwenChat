using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business.Crawler;
using ARW.Model.Models.Business;
using ARW.Model.Vo.Crawler;
using ARW.Service.Business.IBusinessService.Crawler;
using ARW.Model.Models.Business.Crawler;
using ARW.Repository.Business;
using ARW.Repository;
using AngleSharp.Html.Parser;
using RestSharp;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 不羞涩接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(ICrawlService), ServiceLifetime = LifeTime.Transient)]
    public class CrawlServiceImpl : BaseService<Crawl>, ICrawlService
    {
        private readonly CrawlRepository _crawlRepository;

        public CrawlServiceImpl(CrawlRepository crawlRepository)
        {
            this._crawlRepository = crawlRepository;
        }


        public string LoadHTML(string url)
        {
           //return _crawlRepository.LoadBuxiuseHTML(url);
           return _crawlRepository.LoadYellowHTML(url);

        }

        public int ParseMovies(string html)
        {
            //return _crawlRepository.ParseBuxiusePic(html);
            return _crawlRepository.ParseYellowPic(html);
        }

        public List<CrawlVo> GetBuxiuseList(CrawlQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<Crawl>();

            //搜索条件查询语法参考Sqlsugar
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Name), s => s.Name.Contains(parm.Name));
                                                
            var query = _crawlRepository
                .Queryable()
                .Where(predicate.ToExpression())
                .Select((s) => new CrawlVo
                {
                    Id = s.Id,
                    Name = s.Name,
                    Intro = s.Intro,
                    Cover = s.Cover,
                    Link = s.Link,
                    Type = s.Type,
                    PublishTime = s.PublishTime,
                });


            return query.ToList();
        }


    }
}
