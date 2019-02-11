namespace Sales.Services
{
    using Common.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {


        public async Task<Response> CheckConnection()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,

                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if(!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
               
            }
            return new Response
            {
                IsSuccess=true,
            };
        }


        public async Task<Response> GetList<T>(string urlBase, string prefix,  string controller)
        {
            try
            {
                var datos = $"{urlBase}{prefix}{controller}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(datos);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess=false,
                        Message=answer
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var datos = $"{urlBase}{prefix}{controller}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.GetAsync(datos);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    
        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model )
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var datos = $"{urlBase}{prefix}{controller}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(datos, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var datos = $"{urlBase}{prefix}{controller}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(datos, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id )
        {
            try
            {
                var datos = $"{urlBase}{prefix}{controller}/{id}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(datos);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var datos = $"{urlBase}{prefix}{controller}/{id}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var response = await client.DeleteAsync(datos);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var datos = $"{urlBase}{prefix}{controller}/{id}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response = await client.PutAsync(datos, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var datos = $"{urlBase}{prefix}{controller}/{id}";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PutAsync(datos, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);

                return new Response
                {
                    IsSuccess = true,
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<TokenResponse> GetToken(
             string urlBase,
             string username,
             string password)
        {
            try
            {
                var datos = $"{urlBase}/Token";
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(datos,
                    new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
                    username, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }


    }
}
