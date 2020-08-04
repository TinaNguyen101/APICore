using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class PagingRequest<T>
    {
        [JsonProperty(PropertyName = "sortColumn")]
        public string sortColumn
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "sortDir")]
        public string sortDir
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pageNumber")]
        public int pageNumber
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pageSize")]
        public int pageSize
        {
            get;
            set;
        }
        
        [JsonProperty(PropertyName = "filters")]
        public T Filters
        {
            get;
            set;
        }
    }
}
