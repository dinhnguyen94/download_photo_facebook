using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading;

namespace Download_Photo_Facebook
{
    public class FacebookRequest
    {
        private readonly HttpClient _httpClient;

        public FacebookRequest()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v6.0/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<string>> GetPhotosUploaded(string accessToken, string id)
        {
            Console.WriteLine("Dang get link hinh user tu upload");
            List<string> listDirectLinkPhotos = new List<string>();
            try
            {
                var endpoint = id + "/photos/uploaded";
                var result = await FacebookGetAsync<dynamic>(
                    accessToken, endpoint, "limit=9999");

                
                var result_data_1 = result.data;
                Console.WriteLine("Tong cong co {0} hinh", result_data_1.Count);
                int count = 1;
                foreach (var itemPhotos in result_data_1)
                {
                    var photos = new TypePhoto();
                    photos.id = itemPhotos.id;
                    var endpoint_2 = photos.id;
                    try
                    {
                        var result_2 = await FacebookGetAsync<dynamic>(accessToken, endpoint_2, "fields=images");

                        int height = 0;
                        int heightmax = 0;
                        var listimages = result_2.images;
                        foreach (var images in listimages)
                        {
                            height = images.height;
                            if (height > heightmax)
                            {
                                string directLink = images.source;
                                listDirectLinkPhotos.Add(directLink);
                                heightmax = height;
                            }
                        }
                    }
                    catch (HttpRequestException)
                    {
                        Console.WriteLine("Loi internet roi, thu lai sau nhe");
                    }
                    Thread.Sleep(1000);
                    count++;
                    if (count % 10 == 0)
                    {
                        Console.WriteLine("Da get duoc {0} direct link, vui long cho...", count);
                    }
                }
                Console.WriteLine("Da get link xong hinh user tu upload");
                Console.WriteLine("Dang get link hinh user bi tag");

                var endpoint_3 = id + "/photos";
                var result_4 = await FacebookGetAsync<dynamic>(
                    accessToken, endpoint_3, "limit=9999");

                var result_data_4 = result_4.data;
                Console.WriteLine("Tong cong co {0} hinh", result_data_4.Count);

                foreach (var itemPhotos in result_data_4)
                {
                    var photos = new TypePhoto();
                    photos.id = itemPhotos.id;
                    var endpoint_2 = photos.id;
                    try
                    {
                        var result_2 = await FacebookGetAsync<dynamic>(accessToken, endpoint_2, "fields=images");

                        int height = 0;
                        int heightmax = 0;
                        var listimages = result_2.images;
                        foreach (var images in listimages)
                        {
                            height = images.height;
                            if (height > heightmax)
                            {
                                string directLink = images.source;
                                listDirectLinkPhotos.Add(directLink);
                                heightmax = height;
                            }
                        }
                    }
                    catch (HttpRequestException)
                    {
                        Console.WriteLine("Loi internet roi, thu lai sau nhe");
                    }
                    Thread.Sleep(1000);

                    count++;
                    if (count % 10 == 0)
                    {
                        Console.WriteLine("Da get duoc {0} direct link, vui long cho...", count);
                    }
                }
            }

            catch (HttpRequestException)
            {
                Console.WriteLine("Loi internet roi, thu lai sau nhe");
            }

            return listDirectLinkPhotos;
        }

        public async Task<T> FacebookGetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
