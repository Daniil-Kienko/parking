using System;
using System.Net.Http;
using System.Net.Http.Json;
using CoolParking.App.DTO;
using Newtonsoft.Json;

namespace CoolParking.App.Services
{
    class HttpService : IDisposable
    {
        /// <summary>
        // The client works synchronously because we need to wait for a
        // response from the server, display it and only then display
        // the menu.If you work asynchronously, then while sending a request,
        // the thread will not wait for its response and will immediately display
        // a menu and only after that the result of the request is displayed in
        // the line in which user input is expected 
        /// </summary>
        private HttpClient client;

        public HttpService(HttpClientHandler clientHandler)
        {
            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(Settings.LINK);
        }

        /// <summary>
        /// Send GET request to the specified link
        /// </summary>
        /// <param name="link">Request link</param>
        /// <returns>Response string</returns>
        public string GetData(string link)
        {
            var response = client.GetAsync(link).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponseContract errorData = JsonConvert.DeserializeObject<ErrorResponseContract>(responseString);
                throw new InvalidOperationException(errorData.Message);
            }
            return responseString;
        }

        /// <summary>
        /// Send GET request to the specified link and deserialize the result
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="link">Request link</param>
        /// <returns>Deserialized result</returns>
        public T GetDataDeserialized<T>(string link)
        {
            return JsonConvert.DeserializeObject<T>(GetData(link));
        }

        /// <summary>
        /// Send POST request to the specified link
        /// </summary>
        /// <param name="link">Request link</param>
        /// <param name="data">Data to send with request</param>
        /// <returns>Response string</returns>
        public string PostData(string link, object data)
        {
            var response = client.PostAsJsonAsync(link, data).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponseContract errorData = JsonConvert.DeserializeObject<ErrorResponseContract>(responseString);
                throw new ArgumentException(errorData.Message);
            }
            return responseString;
        }

        /// <summary>
        /// Send POST request to the specified link and deserialize the result
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="link">Request link</param>
        /// <param name="data">Data to send with request</param>
        /// <returns>Deserialized result</returns>
        public T PostDataAndDeserializeResponse<T>(string link, object data)
        {
            return JsonConvert.DeserializeObject<T>(PostData(link, data));
        }

        /// <summary>
        /// Send PUT request to the specified link
        /// </summary>
        /// <param name="link">Request link</param>
        /// <param name="data">Data to send with request</param>
        /// <returns>Response string</returns>
        public string PutData(string link, object data)
        {
            var response = client.PutAsJsonAsync(link, data).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponseContract errorData = JsonConvert.DeserializeObject<ErrorResponseContract>(responseString);
                throw new ArgumentException(errorData.Message);
            }
            return responseString;
        }

        /// <summary>
        /// Send PUT request to the specified link and deserialize the result
        /// </summary>
        /// <typeparam name="T">Type for deserialization</typeparam>
        /// <param name="link">Request link</param>
        /// <param name="data">Data to send with request</param>
        /// <returns>Deserialized result</returns>
        public T PutDataAndDeserializeResponse<T>(string link, object data)
        {
            return JsonConvert.DeserializeObject<T>(PutData(link, data));
        }

        /// <summary>
        /// Send DELETE request to the specified link
        /// </summary>
        /// <param name="link">Request link</param>
        public void DeleteData(string link)
        {
            var response = client.DeleteAsync(link).Result;
            if (!response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                ErrorResponseContract errorData = JsonConvert.DeserializeObject<ErrorResponseContract>(responseString);
                throw new InvalidOperationException(errorData.Message);
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
