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
    [AppService(ServiceType = typeof(IGroupUserService), ServiceLifetime = LifeTime.Transient)]
    public class GroupUserServiceImpl : BaseService<GroupUser>, IGroupUserService
    {
        private GroupUserRepository _GroupUserRepository; 

        public GroupUserServiceImpl(GroupUserRepository GroupUserRepository)
        {
            _GroupUserRepository = GroupUserRepository;
        }

        //public PagedInfo<GroupVo> GetGroupList(GroupQueryDto parm)
        //{
        //    var query = _GroupUserRepository
        //        .Queryable()
        //        .LeftJoin<ChatUser>((s, c) => s.FriendsGuId == c.ChatUserGuId)
        //        .Where(s => s.UserGuId == parm.UserGuId)
        //        .Select((s, c) => new FriendsVo
        //        {
        //            FriendGuId = s.FriendsGuId,
        //            FriendName = c.ChatUserName,
        //            FriendNickName = c.ChatUserNickName,
        //            FriendImg = c.ChatUserImg,
        //            FriendNote = s.FriendsNote,
        //            CreateTime = s.Create_time,
        //            Sex = c.Sex,
        //            Age = c.Age,
        //            Phone = c.Phone,
        //            Email = c.Email,
        //            Status = c.Status,
        //        });


        //    return query.ToPage(parm);
        //}

    }
}
