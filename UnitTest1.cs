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
            
            /*
            var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            var result = output["author"];
            */
            JObject obs = JObject.Parse(response.Content);
            
            Assert.That(obs["author"].ToString(), Is.EqualTo("Maximiliyano"), "Author is not correct");
        }
    }
}
