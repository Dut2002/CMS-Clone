using System.Net;

namespace WebClient.Connection
{
	public interface ICallApi
	{
		public Task<(HttpStatusCode, string)> Get(string url);
		public Task<(HttpStatusCode, string)> Post<T>(string url, T data);
		public Task<(HttpStatusCode, string)> PostFile<T>(string url, Stream fileStream, string filename, T data);
		public Task<(HttpStatusCode, string)> Put<T>(string url, T data);
		public Task<(HttpStatusCode, string)> Delete(string url);
	}

}
