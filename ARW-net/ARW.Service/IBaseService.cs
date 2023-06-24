using ARW.Model.Models.Business.Crawler;
using ARW.Repository;
using System;
using System.Collections.Generic;

namespace ARW.Service
{
    /// <summary>
    /// 基础服务定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T> : IBaseRepository<T> where T : class, new()
    {
        public virtual string LoadHTML(string url)
        {
            throw new NotImplementedException();
        }

        public virtual int ParseMovies(string html)
        {
            throw new NotImplementedException();
        }
    }
}