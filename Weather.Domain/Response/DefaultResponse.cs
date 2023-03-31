using System.Net;
using Weather.Domain.Interfaces;

namespace Weather.Domain.Response
{
    public class DefaultResponse<T> : IDefaultResponse<T>
    {
        public string? Message { get; set; }
        public HttpStatusCode HttpCode { get; set; }
        public T? Data { get; set; }
    }
}
