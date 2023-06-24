using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Dto.Business.Product
{
    /// <summary>
    /// 产品类型输入对象
    /// </summary>
    public class ProductTypeDto
    {
        [Required(ErrorMessage = "产品类型Id不能为空")]
        public long ProductTypeId { get; set; }
        [Required(ErrorMessage = "产品类型不能为空")]
        public string ProductTypeName { get; set; }
        public long ParentId { get; set; }
    }

    /// <summary>
    /// 产品类型查询对象
    /// </summary>
    public class ProductTypeQueryDto : PagerInfo
    {
    }
}
