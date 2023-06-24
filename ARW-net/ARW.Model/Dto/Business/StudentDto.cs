using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model.Models.Business;

namespace ARW.Model.Dto.Business
{

    /// <summary>
    /// 输入对象
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public string DictType { get; set; }
        public string StudentName { get; set; }
        public string Sex { get; set; }
        public int? Age { get; set; }
        public string StudentImg { get; set; }
        public string StudentTag { get; set; }
        public Service[] StudentService { get; set; }
        public string StudentDescribe { get; set; }
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class StudentQueryDto : PagerInfo
    {
        /// <summary>
        /// 学生ID
        /// </summary>
        public long? StudentId { get; set; }
        
        /// <summary>
        /// 班级ID
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public string DictType { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }
    }

}
