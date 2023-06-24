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
using System.Threading.Tasks;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Repository.Business;
using SqlSugar;
using ARW.Repository;
using ARW.Model.Vo.Chat;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 登录
    /// </summary>
    [AppService(ServiceType = typeof(IChatLoginService), ServiceLifetime = LifeTime.Transient)]
    public class ChatLoginServiceImpl : BaseService<ChatUser>, IChatLoginService
    {
        private ChatRepository _ChatRepository;

        public ChatLoginServiceImpl(ChatRepository chatRepository)
        {
            _ChatRepository = chatRepository;
        }

        /// <summary>
        /// 获取聊天用户列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PagedInfo<ChatUserVo> GetChatUserList(ChatUserQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<ChatUser>();

            //搜索条件查询语法参考Sqlsugar
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatUserName), it => it.ChatUserName.Contains(parm.ChatUserName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ChatUserNickName), it => it.ChatUserName.Contains(parm.ChatUserNickName));
            predicate = predicate.AndIF(parm.ChatUserStatus != null, it => it.Status == parm.ChatUserStatus);


            var query = _ChatRepository
                    .Queryable()
                    .Where(predicate.ToExpression())
                    .Select((s) => new ChatUserVo
                    {
                        ChatUserId = s.ChatUserId,
                        ChatUserGuId = s.ChatUserGuId,
                        ChatUserName = s.ChatUserName,
                        ChatUserNickName = s.ChatUserNickName,
                        ChatUserImg = s.ChatUserImg,
                        Sex = s.Sex,
                        Age = s.Age,
                        Phone = s.Phone,
                        Email = s.Email,
                        Status = s.Status
                    });

            return query.ToPage(parm);
        }


        public Task<ChatUser> FindUserByGuid(long guid)
        {
            var user = _ChatRepository.GetFirstAsync(q => q.ChatUserGuId == guid);
            if (user == null) { throw new CustomException("用户不存在"); }
            return user;
        }

        public ChatUser FindUserByName(string name)
        {
            var user = _ChatRepository.GetFirst(q => q.ChatUserName == name);
            if (user == null) { throw new CustomException("用户不存在"); }
            return user;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        public ChatUser Login(ChatUserDto loginBody)
        {
            //密码md5
            loginBody.Password = NETCore.Encrypt.EncryptProvider.Md5(loginBody.Password);

            ChatUser user = _ChatRepository.Login(loginBody);

            if (user == null || user.ChatUserId <= 0)
            {
                throw new CustomException(ResultCode.LOGIN_ERROR, "用户名或密码错误");
            }

            if (user.Status == 1)
            {
                throw new CustomException(ResultCode.LOGIN_ERROR, "账号已在别处登录");
            }
            user.Status = 1;
            _ChatRepository.Updateable(user);

            return user;
        }

        public ChatUser Register(ChatUserDto dto)
        {
            //密码md5
            dto.Password = NETCore.Encrypt.EncryptProvider.Md5(dto.Password);

            var u = _ChatRepository.GetFirst(q => q.ChatUserName == dto.ChatUserName);

            if (u != null)
            {
                throw new CustomException(ResultCode.LOGIN_ERROR, "用户名已存在");
            }
            if (!Tools.PasswordStrength(dto.Password))
            {
                throw new CustomException("密码强度不符合要求");
            }
            ChatUser user = new()
            {
                Create_time = DateTime.Now,
                ChatUserName = dto.ChatUserName,
                ChatUserNickName = dto.ChatUserNickName,
                ChatUserImg = "http://chat.aerwen.net/dev-api//uploads/20220930/BB374FB4935C5656.jpg",
                Password = dto.Password,
                Status = 1,
                Sex = "1"
            };

            user.ChatUserGuId = _ChatRepository.AddGuid(user);
            return user;

        }
    }
}
