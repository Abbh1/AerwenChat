using Infrastructure;
using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using ARW.Common;
using ARW.Model;
using ARW.Model.System.Dto;
using ARW.Model.System;
using ARW.Repository.System;
using ARW.Service.Business.IBusinessService.Chat;
using ARW.Model.Models.Business.Chat;
using ARW.Model.Chat;
using ARW.Repository.Business.Chat;
using ARW.Model.Vo.Chat;
using SqlSugar;
using ARW.Repository;
using ARW.Model.Models.Business;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 好友
    /// </summary>  
    [AppService(ServiceType = typeof(IChatLogService), ServiceLifetime = LifeTime.Transient)]
    public class ChatLogServiceImpl : BaseService<ChatLog>, IChatLogService
    {
        private ChatLogRepository _ChatLogRepository;
        private GroupUserRepository _GroupUserRepository;

        public ChatLogServiceImpl(ChatLogRepository ChatLogRepository, GroupUserRepository groupUserRepository)
        {
            _ChatLogRepository = ChatLogRepository;
            _GroupUserRepository = groupUserRepository;
        }

        public List<ChatLogListVo> GetChatLogList(ChatLogQueryDto parm)
        {
            var id = parm.UserGuId;
            var q = Context.Ado.SqlQuery<ChatLogListVo>("SELECT\r\n\t`a`.`SenderGuId` AS `SenderGuId`,\r\n\t`a`.`ReceiverGuId` AS `ReceiverGuId`,\r\n\t`a`.`ChatLogContent` AS `ChatLogContent`,\r\n\t`a`.`isRead` AS `IsRead`,\r\n\tDATE_FORMAT(`a`.`ChatLogSendTime`,'%m月%d日 %H:%i') AS `ChatLogSendTime`,\r\n\t`b`.`chat_user_guid` AS `ChatUserGuId`,\r\n\t`b`.`chat_user_name` AS `ChatUserName`,\r\n\t`b`.`chat_user_nickname` AS `ChatUserNickName`,\r\n\t`b`.`chat_user_sex` AS `Sex`,\r\n\t`b`.`chat_user_age` AS `Age`,\r\n\t`b`.`chat_user_img` AS `ChatUserImg`,\r\n\t`b`.`chat_user_phone` AS `Phone`,\r\n\t`b`.`chat_user_email` AS `Email`,\r\n\t`c`.`friends_note` AS `FriendNote`\r\nFROM\r\n\t(\r\n\tSELECT\r\n\t\t* \r\n\tFROM\r\n\t\t( SELECT\r\n\t\t\t`sender_guid` AS `SenderGuId`,\r\n\t\t\t`receiver_guid` AS `ReceiverGuId`,\r\n\t\t\t`chat_log_content` AS `ChatLogContent`,\r\n\t\t\t`isRead` AS `isRead`,\r\n\t\t\t`chat_log_send_time` AS `ChatLogSendTime`\r\n\t\t\tFROM `tb_chat_log` WHERE ( `receiver_guid` = @id ) ORDER BY Create_time DESC Limit 9999 ) MergeTable \r\n\t) a\r\n\tLEFT JOIN `tb_chat_user` b ON ( `a`.`SenderGuId` = `b`.`chat_user_guid` )\r\n\tLEFT JOIN `tb_chat_friends` c ON ( `b`.`chat_user_guid` = `c`.`friends_guid` and c.uesr_guid = @id )\r\ngroup BY\r\n\t`SenderGuId`", new {id = id });
            var q2 = Context.Ado.SqlQuery<ChatLogListVo>("SELECT\r\n\t`a`.`SenderGuId` AS `SenderGuId`,\r\n\t`a`.`ReceiverGuId` AS `ReceiverGuId`,\r\n\t`a`.`ChatLogContent` AS `ChatLogContent`,\r\n\t`a`.`isRead` AS `IsRead`,\r\n\tDATE_FORMAT ( `a`.`ChatLogSendTime`, '%m月%d日 %H:%i' ) AS `ChatLogSendTime`,\r\n\t`a`.`GroupGuId` AS `GroupGuId`,\r\n\t`a`.`GroupImg` AS `GroupImg`,\r\n\t`a`.`GroupName` AS `GroupName`,\r\n\t`a`.`GroupIsRead` AS `GroupIsRead` \r\nFROM\r\n\t(\r\n\tSELECT\r\n\t\t* \r\n\tFROM\r\n\t\t(\r\n\t\tSELECT\r\n\t\t\t`a`.`sender_guid` AS `SenderGuId`,\r\n\t\t\t`a`.`receiver_guid` AS `ReceiverGuId`,\r\n\t\t\t`a`.`chat_log_content` AS `ChatLogContent`,\r\n\t\t\t`a`.`isRead` AS `isRead`,\r\n\t\t\t`a`.`chat_log_send_time` AS `ChatLogSendTime`,\r\n\t\t\t`b`.`group_guid` AS `GroupGuId`,\r\n\t\t\t`b`.`group_img` AS `GroupImg`,\r\n\t\t\t`b`.`group_name` AS `GroupName`,\r\n\t\t\t`c`.`isRead` AS `GroupIsRead` \r\n\t\tFROM\r\n\t\t\t`tb_chat_log` a\r\n\t\t\tLEFT JOIN `tb_chat_group` b ON ( `a`.`receiver_guid` = `b`.`group_guid` )\r\n\t\t\tJOIN `tb_chat_group_user` c ON ( `b`.`group_guid` = `c`.`group_guid` AND c.user_guid = @id ) \r\n\t\tWHERE\r\n\t\t\t( `a`.`receiver_guid` = b.group_guid ) \r\n\t\tORDER BY\r\n\t\t\tchat_log_send_time DESC \r\n\t\t\tLIMIT 9999 \r\n\t\t) MergeTable \r\n\t) a \r\nGROUP BY\r\n\t`ReceiverGuId`", new { id = id });

            var list = new List<ChatLogListVo>();

            foreach (var item in q)
            {
                list.Add(item);
            }

            foreach (var i in q2)
            {
                list.Add(i);
            }

            return list;
        }

        /// <summary>
        /// 添加好友聊天记录
        /// </summary>
        /// <param name="dto"></param>
        public long AddChatFriendLog(ChatLogDto dto)
        {
            var modal = new ChatLog
            {
                ChatLogType = dto.ChatLogType,
                SenderGuId = dto.SenderGuId,
                ReceiverGuId = dto.ReceiverGuId,
                ChatLogContent = dto.ChatLogContent,
                ChatLogSendTime = DateTime.Now,
            };

            try
            {
                var id = _ChatLogRepository.Insertable(modal).ExecuteReturnSnowflakeId();
                return id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取好友聊天记录列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ChatLogVo> GetChatFriendLogList(ChatLogQueryDto parm)
        {

            var predicate = Expressionable.Create<ChatLogVo>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatLogContent), it => it.ChatLogContent.Contains(parm.ChatLogContent));

            var query1 = _ChatLogRepository
                .Queryable()
                .Where(s => s.SenderGuId == parm.UserGuId && s.ReceiverGuId == parm.FriendsGuId)
                .Select((s) => new ChatLogVo
                {
                    ChatLogType = s.ChatLogType,
                    SenderGuId = s.SenderGuId,
                    ReceiverGuId = s.ReceiverGuId,
                    ChatLogContent = s.ChatLogContent,
                    ChatLogSendTime = s.ChatLogSendTime,
                });

            var query2 = _ChatLogRepository
                .Queryable()
                .Where(s => s.SenderGuId == parm.FriendsGuId && s.ReceiverGuId == parm.UserGuId)
                .Select((s) => new ChatLogVo
                {
                    ChatLogType = s.ChatLogType,
                    SenderGuId = s.SenderGuId,
                    ReceiverGuId = s.ReceiverGuId,
                    ChatLogContent = s.ChatLogContent,
                    ChatLogSendTime = s.ChatLogSendTime,
                });

                
            var select = Context.Union(query1, query2).Where(predicate.ToExpression()).OrderBy(s => s.ChatLogSendTime, OrderByType.Desc);

            return select.ToPage(parm);
        }

        /// <summary>
        /// 获取群聊聊天记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ChatLogVo> GetChatGroupLogList(ChatLogQueryDto parm)
        {
            var predicate = Expressionable.Create<ChatLog>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatLogContent), it => it.ChatLogContent.Contains(parm.ChatLogContent));

            var query1 = _ChatLogRepository
               .Queryable()
               .Where(s => s.ReceiverGuId == parm.FriendsGuId)
               .Where(predicate.ToExpression())
               .OrderBy(s => s.ChatLogSendTime, OrderByType.Desc)
               .LeftJoin<ChatUser>((s, c) => s.SenderGuId == c.ChatUserGuId)
               .Select((s,c) => new ChatLogVo
               {
                   ChatLogType = s.ChatLogType,
                   Sender = c,
                   ReceiverGuId = s.ReceiverGuId,
                   ChatLogContent = s.ChatLogContent,
                   ChatLogSendTime = s.ChatLogSendTime,
               })
               ;

            return query1.ToPage(parm);
        }

        /// <summary>
        /// 获取好友历史聊天记录列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<ChatLogVo> GetChatFriendHistoryLogList(ChatLogQueryDto parm)
        {

            var predicate = Expressionable.Create<ChatLogVo>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatLogContent), it => it.ChatLogContent.Contains(parm.ChatLogContent));

            var query1 = _ChatLogRepository
                .Queryable()
                .Where(s => s.SenderGuId == parm.UserGuId && s.ReceiverGuId == parm.FriendsGuId)
                .Select((s) => new ChatLogVo
                {
                    ChatLogType = s.ChatLogType,
                    SenderGuId = s.SenderGuId,
                    ReceiverGuId = s.ReceiverGuId,
                    ChatLogContent = s.ChatLogContent,
                    ChatLogSendTime = s.ChatLogSendTime,
                });

            var query2 = _ChatLogRepository
                .Queryable()
                .Where(s => s.SenderGuId == parm.FriendsGuId && s.ReceiverGuId == parm.UserGuId)
                .Select((s) => new ChatLogVo
                {
                    ChatLogType = s.ChatLogType,
                    SenderGuId = s.SenderGuId,
                    ReceiverGuId = s.ReceiverGuId,
                    ChatLogContent = s.ChatLogContent,
                    ChatLogSendTime = s.ChatLogSendTime,
                });


            var select = Context.Union(query1, query2).Where(predicate.ToExpression()).OrderBy(s => s.ChatLogSendTime, OrderByType.Desc).ToList();

            return select;
        }

        /// <summary>
        /// 获取群聊历史聊天记录
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<ChatLogVo> GetChatGroupLogHistoryList(ChatLogQueryDto parm)
        {
            var predicate = Expressionable.Create<ChatLog>();

            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatLogContent), it => it.ChatLogContent.Contains(parm.ChatLogContent));

            var query1 = _ChatLogRepository
               .Queryable()
               .Where(s => s.ReceiverGuId == parm.FriendsGuId)
               .Where(predicate.ToExpression())
               .OrderBy(s => s.ChatLogSendTime, OrderByType.Desc)
               .LeftJoin<ChatUser>((s, c) => s.SenderGuId == c.ChatUserGuId)
               .Select((s, c) => new ChatLogVo
               {
                   ChatLogType = s.ChatLogType,
                   Sender = c,
                   ReceiverGuId = s.ReceiverGuId,
                   ChatLogContent = s.ChatLogContent,
                   ChatLogSendTime = s.ChatLogSendTime,
               })
               ;

            return query1.ToList();
        }

        /// <summary>
        /// 获取自己发的消息(最新)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ChatLogVo SelfNewMsg(ChatLogQueryDto parm)
        {
            var query1 = _ChatLogRepository
               .Queryable()
               .Where(s => s.SenderGuId == parm.UserGuId && s.ReceiverGuId == parm.FriendsGuId)
               .Select((s) => new ChatLogVo
               {
                   ChatLogType = s.ChatLogType,
                   SenderGuId = s.SenderGuId,
                   ReceiverGuId = s.ReceiverGuId,
                   ChatLogContent = s.ChatLogContent,
                   ChatLogSendTime = s.ChatLogSendTime,
               }).
               OrderBy(s => s.ChatLogSendTime, OrderByType.Desc);

            return query1.First();
        }

        public void Read(ChatLogQueryDto parm)
        {
            _ChatLogRepository.Update(s => s.SenderGuId == parm.FriendsGuId && s.ReceiverGuId == parm.UserGuId,
                f => new ChatLog
                {
                    IsRead = true
                });
        }

        public void GroupRead(ChatLogQueryDto parm)
        {
            var user = _GroupUserRepository.Update(s => s.GroupGuId == parm.FriendsGuId && s.UserGuId == parm.UserGuId,
                f => new GroupUser
                {
                    IsRead = true
                });
        }
    }
}
