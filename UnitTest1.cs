using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RestSharpDemo
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");
            
            var request = new RestRequest("posts/{postid}", Method.Get);
            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);
            
            JObject obs = JObject.Parse(response.Content);
            
            Assert.That(obs["author"].ToString(), Is.EqualTo("Maximiliyano"), "Author is not correct");
        }

        [Test]
        public void PostWithAnonymousBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}/profile", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { name = "Rafaello" });
            request.AddUrlSegment("postid", 1);
            
            var response = client.Execute(request);
            
            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var result = output["name"];
            
            Assert.That(result.ToString(), Is.EqualTo("Rafaello"), "Author is not correct");
        }

        [Test]
        public void PostWithTypeClassBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.Post);
            
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Posts() { id = "8", author = "Automation", title = "RestSharp"});

            var response = client.Execute<Posts>(request);

            Assert.That(response.Data.author, Is.EqualTo("Automation"), "Author is not correct");
        }
        
        [Test]
        public void PostWithAsync()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.Post);
            
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Posts() { id = "11", author = "Automation", title = "RestSharp"});

            var response = ExecuteAsyncRequest<Posts>(client, request).GetAwaiter().GetResult();
            
            Assert.That(response.Data.author, Is.EqualTo("Automation"), "Author is not correct");
        }

        private async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(RestClient client, IRestRequest request) where T: class, new()
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();

            client.ExecuteAsync<T>(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response!";
                    throw new ApplicationException(message, restResponse.ErrorException);
                }
                
                taskCompletionSource.SetResult(restResponse);
            });

            return await taskCompletionSource.Task;
        }
    }
}
