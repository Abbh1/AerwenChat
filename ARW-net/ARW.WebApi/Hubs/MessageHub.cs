using Infrastructure;
using Infrastructure.Constant;
using Infrastructure.Model;
using IPTools.Core;
using Microsoft.AspNetCore.SignalR;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Framework;
using ARW.Model;
using ARW.Model.System;
using ARW.Service.System.IService;
using Quartz.Util;
using Microsoft.AspNetCore.Http;

namespace ARW.Admin.WebApi.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageHub : Hub
    {
        //创建用户集合，用于存储所有链接的用户数据
        private static readonly List<OnlineUsers> clientUsers = new();
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ISysNoticeService SysNoticeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noticeService"></param>
        public MessageHub(ISysNoticeService noticeService)
        {
            SysNoticeService = noticeService;
        }

        private ApiResult SendNotice()
        {
            var result = SysNoticeService.GetSysNotices();

            return new ApiResult(200, "success", result);
        }

        #region 客户端连接

        /// <summary>
        /// 客户端连接的时候调用
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            var ip = HttpContextExtension.GetClientUserIp(App.HttpContext);
            var ip_info = IpTool.Search(ip);

            LoginUser loginUser = JwtUtil.GetLoginUser(App.HttpContext);
            var user = clientUsers.Any(u => u.ConnnectionId == Context.ConnectionId);
            //判断用户是否存在，否则添加集合
            if (!user && Context.User.Identity.IsAuthenticated)
            {
                OnlineUsers users = new(Context.ConnectionId, name, loginUser?.UserId, ip)
                {
                    Location = ip_info.City
                };
                clientUsers.Add(users);
                Console.WriteLine($"{DateTime.Now}：{name},{Context.ConnectionId}连接服务端success，当前已连接{clientUsers.Count}个");
                //Clients.All.SendAsync("welcome", $"欢迎您：{name},当前时间：{DateTime.Now}");
                Clients.All.SendAsync(HubsConstant.MoreNotice, SendNotice());
            }

            Clients.All.SendAsync(HubsConstant.OnlineNum, clientUsers.Count);
            Clients.All.SendAsync(HubsConstant.OnlineUser, clientUsers);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 连接终止时调用。
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = clientUsers.Where(p => p.ConnnectionId == Context.ConnectionId).FirstOrDefault();
            //判断用户是否存在，否则添加集合
            if (user != null)
            {
                clientUsers.Remove(user);
                Clients.All.SendAsync(HubsConstant.OnlineNum, clientUsers.Count);
                Clients.All.SendAsync(HubsConstant.OnlineUser, clientUsers);
                Console.WriteLine($"用户{user?.Name}离开了，当前已连接{clientUsers.Count}个");
            }
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        /// <summary>
        /// 注册信息
        /// </summary>
        /// <param name="connectId"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("SendMessage")]
        public async Task SendMessage(string connectId, string userName, string message)
        {
            Console.WriteLine($"{connectId},message={message}");
            bool isDemoMode = AppSettings.GetAppConfig("DemoMode", true);
            if (isDemoMode)
            {
                await Clients.Caller.SendAsync("receiveChat", new { userName, message = "当前环境为演示环境，消息不会发送给对方" });
                return;
            }
            await Clients.Client(connectId).SendAsync("receiveChat", new { userName, message });
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendChat(string user, string message)
        {
            // 服务端主动调用客户端的方法
            await Clients.All.SendAsync("ReceiveMessage", new { user, message });
        }
    }
}
