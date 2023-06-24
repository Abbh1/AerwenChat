using Newtonsoft.Json;
using SqlSugar;
using System.Collections.Generic;

namespace ARW.Model.Vo
{
    public class ProductTypeVo
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(IsTreeKey = true)]
        public long ProductTypeId { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long ParentId { get; set; }

        public string ProductTypeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [SugarColumn(IsIgnore = true)]
        public List<ProductTypeVo> Children { get; set; }

    }
}
