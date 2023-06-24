using Infrastructure.Attribute;
using System;
using System.Collections.Generic;
using System.Text;
using ARW.Model.System;
using ARW.Repository;
using ARW.Repository.System;
using ARW.Service.System.IService;

namespace ARW.Service.System
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    [AppService(ServiceType = typeof(ISysPostService), ServiceLifetime = LifeTime.Transient)]
    public class SysPostService : BaseService<SysPost>, ISysPostService
    {
        public SysPostRepository PostRepository;
        public SysPostService(SysPostRepository postRepository)
        {
            PostRepository = postRepository;
        }

        /// <summary>
        /// 校验岗位编码是否唯一
        /// </summary>
        /// <param name="sysPost"></param>
        /// <returns></returns>
        public string CheckPostCodeUnique(SysPost post)
        {
            SysPost info = PostRepository.GetFirst(it => it.PostCode.Equals(post.PostCode));
            if (info != null && info.PostId != post.PostId)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        /// <summary>
        /// 校验岗位名称是否唯一
        /// </summary>
        /// <param name="sysPost"></param>
        /// <returns></returns>
        public string CheckPostNameUnique(SysPost post)
        {
            SysPost info = PostRepository.GetFirst(it => it.PostName.Equals(post.PostName));
            if (info != null && info.PostId != post.PostId)
            {
                return UserConstants.NOT_UNIQUE;
            }
            return UserConstants.UNIQUE;
        }

        public List<SysPost> GetAll()
        {
            return PostRepository.GetAll();
        }
    }
}
