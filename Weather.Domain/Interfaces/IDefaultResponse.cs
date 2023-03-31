using System.Net;

namespace Weather.Domain.Interfaces
{
    public interface IDefaultResponse<T>
    {
        public string? Message { get; set; }
        public HttpStatusCode HttpCode { get; set; }
        public T? Data { get; set; }
    }
}
