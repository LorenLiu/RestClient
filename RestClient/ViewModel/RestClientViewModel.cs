using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RestClient.Model;
using RestUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace RestClient.ViewModel
{
    public class RestClientViewModel : ViewModelBase
    {
        private string requestUrl;
        private string responseBody;
        private string responseHeaders;
        private string responseStatus;
        private ObservableCollection<RequestInfo> histories;
        private List<string> httpVerbs;
        private string selectedHttpVerb = "GET";
        private string postData;

        public RestClientViewModel()
        {
            this.SendRequestCmd = new RelayCommand(ExecuteSendRequest, CanExecuteSendRequest);
        }

        public string RequestUrl
        {
            get { return requestUrl; }
            set
            {
                if (requestUrl == value)
                {
                    return;
                }
                requestUrl = value;
                RaisePropertyChanged("RequestUrl");
            }
        }

        public string ResponseStatus
        {
            get { return responseStatus; }
            set
            {
                if (responseStatus == value)
                {
                    return;
                }
                responseStatus = value;
                RaisePropertyChanged("ResponseStatus");
            }
        }

        public string ResponseHeaders
        {
            get { return responseHeaders; }
            set
            {
                if (responseHeaders == value)
                {
                    return;
                }
                responseHeaders = value;
                RaisePropertyChanged("ResponseHeaders");
            }
        }

        public string ResponseBody
        {
            get { return responseBody; }
            set
            {
                if (responseBody == value)
                {
                    return;
                }
                responseBody = value;
                RaisePropertyChanged("ResponseBody");
            }
        }

        public ObservableCollection<RequestInfo> Histories
        {
            get
            {
                if (histories == null)
                {
                    histories = new ObservableCollection<RequestInfo>();
                }
                return histories;
            }
        }

        public List<string> HttpVerbs
        {
            get
            {
                if (httpVerbs == null)
                {
                    httpVerbs = new List<string>() { "GET", "POST", "PUT", "DELETE" };
                }
                return httpVerbs;
            }
        }

        public string SelectedHttpVerb
        {
            get { return selectedHttpVerb; }
            set
            {
                if (selectedHttpVerb == value)
                {
                    return;
                }
                selectedHttpVerb = value;
                RaisePropertyChanged("SelectedHttpVerb");
            }
        }

        public string PostData
        {
            get { return postData; }
            set
            {
                if (postData == value)
                {
                    return;
                }
                postData = value;
                RaisePropertyChanged("PostData");
            }
        }

        public RelayCommand SendRequestCmd
        {
            get;
            private set;
        }

        private void ExecuteSendRequest()
        {
            IResponseInfo result = null;
            try
            {
                result = RestManager.GetInstance().MakeRequest(RequestUrl, SelectedHttpVerb, "text/xml", PostData);
            }
            catch (RestException restEx)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(restEx.Message), "RestException");
                return;
            }
            ResponseBody = result.ResponseData;
            ResponseHeaders = GetResponseHeaders(result.ResponseHeaders);
            ResponseStatus = GetResponseStatus(result.ResponseStatusCode);
            Histories.Insert(0, new RequestInfo() { Url = RequestUrl, Verb = SelectedHttpVerb });
        }

        private string GetResponseHeaders(WebHeaderCollection webHeaderCollection)
        {
            if (webHeaderCollection == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            foreach(var key in webHeaderCollection.AllKeys)
            {
                sb.Append(key.ToString());
                sb.Append(" --> ");
                sb.Append(webHeaderCollection[key].ToString());
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private string GetResponseStatus(HttpStatusCode statusCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(((int)statusCode).ToString());
            sb.Append(" ");
            sb.Append(statusCode.ToString());
            return sb.ToString();
        }

        private bool CanExecuteSendRequest()
        {
            return true;
        }
    }
}
