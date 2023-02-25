using System.Net.Http.Headers;

namespace FlickrNetTest
{
    public static class UrlHelper
    {     
        public static bool Exists(string url)
        {
            using var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;

            using var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

            using var request = new HttpRequestMessage(HttpMethod.Head, url);

            try
            {
                using var response = httpClient.Send(request, HttpCompletionOption.ResponseHeadersRead);
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.GetType() + " thrown.");
                Console.WriteLine("Message:" + exception.Message);
                return false;
            }
        }
    }
}