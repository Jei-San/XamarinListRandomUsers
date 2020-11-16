using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinRandomUsers.Model
{
    public static class RandomUsersApiClient
    {
        private static string BASE_URL = "https://randomuser.me/api/";

        public static IRestResponse GetUsers(string quantity)
        {
            var client = new RestClient(BASE_URL);
            var request = new RestRequest($"?results={quantity}", DataFormat.Json);
            return client.Get(request);
        } 
    }
}
