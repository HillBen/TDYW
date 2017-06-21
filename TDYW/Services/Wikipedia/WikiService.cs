using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace TDYW.Services
{
    public class WikiService : IWikiService
    {
        private static HttpClient _client { get; set; }
        private readonly ILogger _logger;
        private readonly IOptions<ServerOptions> _serverOptionsAccessor;

        public WikiService(ILoggerFactory loggerFactory, IOptions<ServerOptions> serverOptionsAccessor)
        {
            _logger = loggerFactory.CreateLogger<WikiService>();
            _serverOptionsAccessor = serverOptionsAccessor;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_serverOptionsAccessor.Value.WikiApiBaseUrl);
        }
        public List<pageval> FindPeople(string term, int limit = 10, int offset = 0, bool living = true)
        {
            List<pageval> results = new List<pageval>();
            WikiRequest request = new WikiRequest();
            request.SearchString = term;
            request.GetRevision = false;
            WikiResponse response = GetResponseAsync(request).Result;
            if(response != null && response.query != null && response.query.pages != null)
            {
                results = response.query.pages.Values.ToList();
            }
            return results;
            //return results;
            ///w/api.php?action=query&generator=search&gsrlimit=5&prop=pageimages|pageterms&pilimit=5&format=json&wbptterms=description&gsrsearch=hastemplate:Birth_date_and_age%20

            ///w/api.php?action=query&format=json&prop=pageimages%7Cpageterms&generator=search&redirects=1&formatversion=2&piprop=thumbnail&pithumbsize=50&pilimit=10&wbptterms=description&gpssearch=Albert+Ei&gpslimit=10
        }

        public pageval GetPerson(int pageId)
        {
            WikiRequest request = new WikiRequest();
            request.PageId = pageId;
            WikiResponse response = GetResponseAsync(request).Result;
            if (response != null && response.query != null && response.query.pages != null)
            {
                return response.query.pages.Values.FirstOrDefault();
            }
            return null;
        }

        private async Task<WikiResponse> GetResponseAsync(WikiRequest request)
        {

            string json = JsonConvert.SerializeObject(request);
            HttpResponseMessage response;
            HttpRequestMessage message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                message.Content = content;
                response = await _client.SendAsync(message).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                json =  await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return JsonConvert.DeserializeObject<WikiResponse>(json);

            //if (String.IsNullOrWhiteSpace(name)){
            //  result = _client.PostAsync(db, new StringContent(json, null, "application/json")).Result;
            //}else {
            //  result = clientSetup().PutAsync(db + String.Format("/{0}", name), new StringContent(json, null, "application/json")).Result;
            //}
            //return result;
            //WikiResponse r = null;
            //return r;
        }

    }
}
