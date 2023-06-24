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

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 好友
    /// </summary>
    [AppService(ServiceType = typeof(IGroupService), ServiceLifetime = LifeTime.Transient)]
    public class GroupServiceImpl : BaseService<Group>, IGroupService
    {
        private GroupRepository _GroupRepository;

        public GroupServiceImpl(GroupRepository GroupRepository)
        {
            _GroupRepository = GroupRepository;
        }

        public PagedInfo<GroupVo> GetGroupList(GroupQueryDto parm)
        {
            var query = _GroupRepository
                .Queryable()
                .LeftJoin<GroupUser>((s, c) => s.GroupGuId == c.GroupGuId)
                .LeftJoin<ChatUser>((s, c, d) => c.UserGuId == d.ChatUserGuId)
                .Where((s, c, d) => d.ChatUserGuId == parm.UserGuId)
                .Select((s, c, d) => new GroupVo
                {
                    GroupGuId = s.GroupGuId,
                    GroupName = s.GroupName,
                    GroupImg = s.GroupImg,
                    GroupNum = SqlFunc.Subqueryable<GroupUser>().LeftJoin<Group>((a,b) => a.GroupGuId == b.GroupGuId).Where(a => a.GroupGuId == s.GroupGuId).Count(),
                    GroupUserList = d,
                    CreateTime = s.Create_time,
                });


            return query.ToPage(parm);
        }

    }
}
