using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business.Crawler;
using AngleSharp.Html.Parser;
using RestSharp;

namespace ARW.Repository.Business
{
    /// <summary>
    /// 爬虫仓储服务
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class CrawlRepository : BaseRepository<Crawl>
    {
        protected static HtmlParser htmlParser = new HtmlParser();


        // 不羞涩
        public string LoadBuxiuseHTML(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("https://www.buxiuse.com/", Method.Get);
                request.AddHeader("cookie", "SESSION=YmNhNjgxNTctNzk3MS00YTVkLThmM2YtMDBjYzhjZDNiNzNm; Hm_lvt_479b5d690f3b5d1eae450ce953f78480=1660291808,1660530399; Hm_lpvt_479b5d690f3b5d1eae450ce953f78480=1660530399");
                request.AddHeader("accept-encoding", "gzip, deflate, br");
                request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                request.AddHeader("upgrade-insecure-requests", "1");
                request.AddHeader("referer", "https://www.buxiuse.com/");
                request.AddHeader("cache-control", "max-age=0,no-cache");
                request.AddHeader("authority", "www.buxiuse.com");
                RestResponse response = client.Execute(request);
                return response.IsSuccessful ? response.Content : "";

            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadHTML fail,url:{url},ex:{ex.ToString()}");

                return string.Empty;
            }
        }
        public int ParseBuxiusePic(string html)
        {
            var dom = htmlParser.ParseDocument(html);
            var movies = new List<Crawl>();
            var lis = dom.QuerySelectorAll(".panel-body li");
            foreach (var li in lis)
            {

                var onlineURL = "https://www.buxiuse.com" + li.QuerySelector("a").GetAttribute("href");
                var movie = new Crawl()
                {
                    Name = li.QuerySelector("img").GetAttribute("title"),
                    Cover = li.QuerySelector("img").GetAttribute("src"),
                    Link = onlineURL,
                    Type = "图片",
                    PublishTime = DateTime.Now,
                };
                //Context.Insertable(movie).ExecuteReturnIdentity();
                Add(movie);
                movies.Add(movie);
            }

            if (movies.Count == 0)
                return 0;
            else
                return 1;
        }


        // Yellow片
        public string LoadYellowHTML(string url)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Get);
                request.AddHeader("cookie", "Hm_lvt_07f2c7e5bd9592209d606f0184fc3d8f=1660568257; recente=%5B%7B%22vod_name%22%3A%22%E5%9B%BD%E4%BA%A7AV%E5%89%A7%E6%83%85-%E4%B8%88%E5%A4%AB%E7%9A%84%22%2C%22vod_url%22%3A%22https%3A%2F%2Frra5ii1x6k3in.com%3A58002%2Findex.php%2Fvod%2Fplay%2Fid%2F138577%2Fsid%2F1%2Fnid%2F1.html%22%2C%22vod_part%22%3A%22%E7%AC%AC1%E9%9B%86%22%7D%5D; Hm_lvt_36ab7abfe863c7133f2af34068ebfc82=1660568383; Hm_lpvt_36ab7abfe863c7133f2af34068ebfc82=1660568383; Hm_lpvt_07f2c7e5bd9592209d606f0184fc3d8f=1660570678");
                request.AddHeader("accept-encoding", "gzip, deflate, br");
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36");
                request.AddHeader("upgrade-insecure-requests", "1");
                request.AddHeader("referer", url);
                request.AddHeader("cache-control", "max-age=0");
                RestResponse response = client.Execute(request);
                return response.IsSuccessful ? response.Content : "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadHTML fail,url:{url},ex:{ex.ToString()}");

                return string.Empty;
            }
        }
        public int ParseYellowPic(string html)
        {
            var dom = htmlParser.ParseDocument(html);
            var movies = new List<Crawl>();
            var lis = dom.QuerySelectorAll(".stui-pannel_bd")?.SelectMany(div => div.QuerySelectorAll("li"));
            foreach (var li in lis)
            {
                var onlineURL = "https://rra5ii1x6k3in.com:58002/" + li.QuerySelector("a").GetAttribute("href");
                var name = li.QuerySelector("a").GetAttribute("title");

                string[] arr = { "福利姬","swag","丝" ,"jk","可爱","制服","网红"};

                foreach (var item in arr)
                {
                    if (name.Contains(item) == true)
                    {
                        var movie = new Crawl()
                        {
                            Name = name,
                            Link = onlineURL,
                            Type = "Yellow",
                            Cover = li.QuerySelector("a").GetAttribute("data-original"),
                        };
                        Add(movie);
                        movies.Add(movie);
                    }
                    else
                    {
                        continue;
                    }
                }

            }

            if (movies.Count == 0)
                return 0;
            else
                return 1;
        }


    }
}
