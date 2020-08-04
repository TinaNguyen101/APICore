using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.DTO
{
    public class PagingResponse<T>
    {
        [JsonProperty(PropertyName = "itemscount")]
        public int itemscount
        {
            get;
            set;
        }
       
        [JsonProperty(PropertyName = "items")]
        public T[] items
        {
            get;
            set;
        }
    }
}
