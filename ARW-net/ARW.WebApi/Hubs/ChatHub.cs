using Infrastructure;
using Infrastructure.Constant;
using Infrastructure.Model;
using IPTools.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Framework;
using ARW.Model;
using ARW.Model.System;
using ARW.Service.System.IService;
using JinianNet.JNTemplate;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Net;
using ARW.Service.Business.IBusinessService.Chat;
using ARW.Model.Models.Business.Chat;
using Infrastructure.Extensions;
using ARW.Repository.Business.Chat;

namespace ARW.Admin.WebApi.Hubs
{
    /// <summary>
    /// 聊天室SignalR
    /// </summary>
    public class ChatHub : Hub
    {
        private static readonly List<OnlineUsersForChat> clientusers = new();
        private readonly IChatLoginService _ChatLoginService;
        private readonly IGroupUserService _GroupUserService;

        public ChatHub(IChatLoginService chatLoginService, IGroupUserService groupUserService)
        {
            this._ChatLoginService = chatLoginService;
            _GroupUserService = groupUserService;
        }

        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //private ApiResult SendNotice()
        //{
        //    var result = SysNoticeService.GetSysNotices();

        //    return new ApiResult(200, "success", result);
        //}

        #region 客户端连接

        /// <summary>
        /// 客户端连接的时候调用
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var username = httpContext.Request.Cookies["username"];
            if (username == null)
            {
                throw new Exception("系统异常连接失败");
            }
            var userInfo = _ChatLoginService.FindUserByName(username);

            _ChatLoginService.Update(s => s.ChatUserName == username,
                f => new ChatUser
                {
                    Status = 1
                });

            var user = clientusers.Any(u => u.ConnectionId == Context.ConnectionId);
            ////判断用户是否存在，否则添加集合
            if (user == false)
            {
                OnlineUsersForChat users = new(Context.ConnectionId, userInfo.ChatUserName, userInfo.ChatUserGuId)
                {
                };

                clientusers.Add(users);
            }

            foreach (var item in clientusers)
            {
                Console.WriteLine(item);
            }

            //Clients.All.SendAsync(HubsConstant.OnlineNum, clientUsers.Count);
            Clients.All.SendAsync("onlineChatUser", clientusers);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 连接终止时调用。
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var username = httpContext.Request.Cookies["username"];

            if (username == null)
            {
                throw new Exception("系统异常连接失败");
            }

            // 修改用户登录状态
            _ChatLoginService.Update(s => s.ChatUserName == username,
               f => new ChatUser
               {
                   Status = 0
               });

            var user = clientusers.Where(p => p.ConnectionId == Context.ConnectionId).FirstOrDefault();
            ////判断用户是否存在，否则添加集合
            if (user != null)
            {
                clientusers.Remove(user);
            }
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        /// <summary>
        /// 好友发送信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendFriendsChat(string selfConnectionId, string connectId, string sender, string receiver, string message)
        {
            if (string.IsNullOrEmpty(connectId))
            {
                throw new CustomException("好友不在线，请留言！");
            }

            // 服务端主动调用客户端的方法
            // 向指定用户(connectId)发送指定消息
            // 监听接受方法("ReceiveMessage")来获取消息 -> ( new { sender, receiver, message } )
            await Clients.Client(connectId).SendAsync("ReceiveMessage", new { sender, receiver, message });
            await Clients.Client(selfConnectionId).SendAsync("ReceiveMessage", new { sender, receiver, message });
        }


        /// <summary>
        /// 进入指定组
        /// </summary>
        /// <param name="connectId"></param>
        /// <param name="roomName">组的名称</param>
        [HubMethodName(nameof(EnterRoom))]
        public void EnterRoom(string connectId, string roomName)
        {
            Groups.AddToGroupAsync(connectId, roomName);
        }

        /// <summary>
        /// 群聊天(发送信息)
        /// </summary>
        /// <param name="roomName"></param>
        public async Task SendGroupChat(string roomName, string groupGuId, string selfConnectionId, string senderId, string receiver, string message)
        {
            var guid = senderId.ParseToLong();
            var groupguid = groupGuId.ParseToLong();
            var sender = _ChatLoginService.FindUserByGuid(guid).Result;

            await Clients.Group(roomName)
                .SendAsync("groupMessages", new { roomName, sender, receiver, message });

            _GroupUserService.Update(s => s.GroupGuId == groupguid,
            f => new GroupUser
            {
                IsRead = false
            });

            //await Clients.Client(selfConnectionId).SendAsync("groupMessages", new { sender, receiver, message });
        }

    }
}
