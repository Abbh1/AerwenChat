using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARW.Model.Models.Business.Product
{
    [SugarTable("tb_product_type")]
    public class ProductType : BusinessBase
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsPrimaryKey = true, ColumnName = "product_type_id")]
        public long ProductTypeId{ get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [SugarColumn(ColumnDescription = "产品类型",ColumnName = "product_type_name")]
        public string ProductTypeName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [SugarColumn(ColumnDescription = "父级Id", IsNullable = true, ColumnName = "parent_id")]
        public long ParentId { get; set; }

        /// <summary>
        /// 子级内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProductType> Children { get; set; }
    }
}
