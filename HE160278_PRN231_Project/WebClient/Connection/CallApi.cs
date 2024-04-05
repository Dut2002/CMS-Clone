using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.Connection
{
	public class CallApi : ICallApi
	{
		private HttpClient _httpClient;
		private string _root = "https://localhost:7081/api";

		public CallApi(HttpClient httpClient)
		{
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
            _httpClient.DefaultRequestHeaders.Connection.Add("Keep-Alive");
            _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");


            _httpClient.DefaultRequestHeaders.Accept.Clear();

        }

        public async Task<(HttpStatusCode, string)> Get(string url)
		{
			HttpResponseMessage response = await _httpClient.GetAsync(_root + url);
			string content = await response.Content.ReadAsStringAsync();
			return (response.StatusCode, content);
		}

        public async Task<(HttpStatusCode, string)> Post<T>(string url, T data)
		{
			string jsonContent = JsonConvert.SerializeObject(data);
			HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync(_root + url, content);
			string responseBody = await response.Content.ReadAsStringAsync();
			return (response.StatusCode, responseBody);
		}

		public async Task<(HttpStatusCode, string)> PostFile<T>(string url, Stream fileStream, string filename, T data)
		{
			using (var content = new MultipartFormDataContent())
			{
				// Thêm file vào yêu cầu
				var fileContent = new StreamContent(fileStream);
				fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
				{
					Name = "file",
					FileName = filename
				};
				content.Add(fileContent);

				// Chuyển đổi dữ liệu thành chuỗi JSON và thêm vào yêu cầu
				string jsonData = JsonConvert.SerializeObject(data);
				var dataContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
				content.Add(dataContent, "data");

				// Gửi yêu cầu POST đến API
				HttpResponseMessage response = await _httpClient.PostAsync(_root + url, content);

				// Đọc và trả về kết quả từ phản hồi của API
				string responseBody = await response.Content.ReadAsStringAsync();
				return (response.StatusCode, responseBody);
			}
		}




		public async Task<(HttpStatusCode, string)> Put<T>(string url, T data)
		{
			string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(data);
			HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PutAsync(_root + url, content);
			string responseBody = await response.Content.ReadAsStringAsync();
			return (response.StatusCode, responseBody);
		}

		public async Task<(HttpStatusCode, string)> Delete(string url)
		{
			HttpResponseMessage response = await _httpClient.DeleteAsync(_root + url);
			string content = await response.Content.ReadAsStringAsync();
			return (response.StatusCode, content);
		}
	}
}