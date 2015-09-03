using System;
using System.IO;
using System.Net;
using System.Text;

namespace RestUtil
{
    public class RestManager
    {
        private class RespInfo : IResponseInfo
        {

            public string ResponseData
            {
                get;
                set;
            }

            public HttpStatusCode ResponseStatusCode
            {
                get;
                set;
            }

            public WebHeaderCollection ResponseHeaders
            {
                get;
                set;
            }
        }

        private static RestManager instance = null;

        private RestManager()
        {

        }

        public static RestManager GetInstance()
        {
            if (instance == null)
            {
                instance = new RestManager();
            }
            return instance;
        }

        // This method Reference to http://www.codeproject.com/Tips/497123/How-to-make-REST-requests-with-Csharp
        public IResponseInfo MakeRequest(string url, string httpVerb = "GET", string contentType = "text/xml", string postData = "")
        {
            try
            {
                var req = WebRequest.Create(url) as HttpWebRequest;
                req.Proxy.Credentials = CredentialCache.DefaultCredentials;

                req.Method = httpVerb;
                req.ContentLength = 0;
                req.ContentType = contentType;

                if (!string.IsNullOrEmpty(postData) && httpVerb == "POST")
                {
                    var encoding = new UTF8Encoding();
                    var bytes = encoding.GetBytes(postData);
                    req.ContentLength = bytes.Length;

                    using (var writeStream = req.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var resp = req.GetResponse() as HttpWebResponse)
                {
                    var responseValue = new RespInfo();
                    responseValue.ResponseStatusCode = resp.StatusCode;
                    responseValue.ResponseHeaders = resp.Headers;

                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", resp.StatusCode);
                        throw new RestException(message);
                    }

                    using (var responseStream = resp.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue.ResponseData = reader.ReadToEnd();
                            }
                        }
                    }

                    return responseValue;
                }
            }
            catch (UriFormatException uriFormatEx)
            {
                throw new RestException("The url input is invalid!", uriFormatEx);
            }
            catch (WebException webEx)
            {
                var responseValue = new RespInfo();
                HttpWebResponse resp = webEx.Response as HttpWebResponse;
                if (resp == null)
                {
                    responseValue.ResponseData = webEx.Message;
                    return responseValue;
                }
                responseValue.ResponseStatusCode = resp.StatusCode;
                responseValue.ResponseHeaders = resp.Headers;

                using (var responseStream = resp.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue.ResponseData = reader.ReadToEnd();
                        }
                    }
                }
                return responseValue;
            }
            catch (ArgumentNullException anex)
            {
                throw new RestException("Please input an url!", anex);
            }
        }
    }
}
