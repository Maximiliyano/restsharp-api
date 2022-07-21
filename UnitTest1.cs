using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;

namespace RestSharpDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}", Method.Get);
            request.AddUrlSegment("postid", 1);

            var content = client.Execute(request).Content;
        }
    }
}
