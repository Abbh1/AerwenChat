using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Dto.Business
{

    /// <summary>
    /// 输入对象
    /// </summary>
    public class ClassDto
    {
        public long ClassId { get; set; }
        [Required(ErrorMessage = "班级名称不能为空")]
        public string ClassName { get; set; }
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    public class ClassQueryDto : PagerInfo
    {
        public long? ClassId { get; set; }
        public string ClassName { get; set; }
    }

}
