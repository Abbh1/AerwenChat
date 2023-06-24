using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Service.Business.IBusinessService;
using System.Configuration;
using Microsoft.AspNetCore.SignalR;
using ARW.Admin.WebApi.Hubs;
using ARW.Model.Chat;
using ARW.Service.Business.IBusinessService.Chat;

namespace ARW.Admin.WebApi.Controllers.Business.Api.Chat
{
    /// <summary>
    /// 聊天室控制器
    /// </summary>
    [Route("business/[controller]")]
    public class ChatController : BaseController
    {

        private readonly ILogger<ChatController> _logger;
        private readonly IHubContext<MessageHub> _hubContext;

        public ChatController(IHubContext<MessageHub> hubContext, ILogger<ChatController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }


        //[HttpGet]
        //public string Get()
        //{
        //    //var message = new NotificationModel { Gid = Guid.NewGuid() };
        //    //var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
        //    ////_hubContext.Clients.All.SendAsync("NotificationCenter", json);
        //    //_hubContext.Clients.All.SendAsync("SendMessage", );
        //    //return Convert.ToString(json);
        //}



        /// <summary>
        /// 获取聊天室列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        /// <returns></returns>
        //[HttpGet("getCrawlList")]
        //public IActionResult GetCrawlist([FromQuery] CrawlQueryDto parm)
        //{
        //    var res = _crawlService.GetBuxiuseList(parm);
        //    return SUCCESS(res);
        //}

    }
}
