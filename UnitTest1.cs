using System.Collections.Generic;
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
            request.AddBody(new Posts() { id = "4", author = "Automation", title = "RestSharp"});

            var response = client.Execute(request);
            
            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var result = output["author"];
            
            Assert.That(result.ToString(), Is.EqualTo("Automation"), "Author is not correct");
        }
    }
}
