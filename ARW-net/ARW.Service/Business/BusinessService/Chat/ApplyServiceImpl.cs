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
using Mapster;
using Microsoft.AspNetCore.Http;
using ARW.Model.Models.Business;
using ARW.Repository.Business;
using SqlSugar;
using ARW.Repository;
using ARW.Model.Vo;
using ARW.Model.Vo.Chat;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 申请
    /// </summary>
    [AppService(ServiceType = typeof(IApplyService), ServiceLifetime = LifeTime.Transient)]
    public class ApplyServiceImpl : BaseService<Apply>, IApplyService
    {
        private ChatApplyRepository _ChatRepository;

        public ApplyServiceImpl(ChatApplyRepository chatRepository)
        {
            _ChatRepository = chatRepository;
        }

        public PagedInfo<ChatApplyRequestVo> GetApplyList(ApplyQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<Apply>();

            var query = _ChatRepository
                .Queryable()
                .LeftJoin<ChatUser>((s, c) => s.SenderGuId == c.ChatUserGuId)
                .Where(s => s.ReceiverGuId == parm.UserGuId || ( s.SenderGuId == parm.UserGuId  && s.IsAgree == false))
                .Select((s, c) => new ChatApplyRequestVo
                {
                    SenderGuId = s.SenderGuId,
                    SenderName = c.ChatUserNickName,
                    SenderImg = c.ChatUserImg,
                    Postscript = s.Postscript,
                    Reply = s.Reply,
                    IsAgree = s.IsAgree,
                    IsGroupApply = s.IsGroupApply,
                    IsRead = s.IsRead,
                    SendTime = s.Create_time,
                });


            return query.ToPage(parm);
        }

    }
}
