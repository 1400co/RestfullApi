using Template.Core.CustomEntities;
using System.Collections.Generic;

namespace Template.Api.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Metadata? Meta { get; set; }
        public List<LinkInfo>? Links { get; set; }
    }
}
