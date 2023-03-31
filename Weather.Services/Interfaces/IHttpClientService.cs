namespace Weather.Services.Interfaces
{
    public interface IHttpClientService
    {
        public Task<TD?> Get<TD>(string url);
    }
}
