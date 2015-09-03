using System.Net;

namespace RestUtil
{
    public interface IResponseInfo
    {
        string ResponseData { get; set; }
        HttpStatusCode ResponseStatusCode { get; set; }
        WebHeaderCollection ResponseHeaders { get; set; }
    }
}
