using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NDrop
{
    public class Account
    {
        public string address;
        public List<Coin> coins;
        public PubKey public_key;
        public string account_number;
        public string sequence;

        public Account() { }

        public static Account Query(Network network, string address)
        {
            Task<IRestResponse> task = Task.Run(async () => await QueryAsync(network, address));            
            return JObject.Parse(task.Result.Content)["result"]["value"].ToObject<Account>();
        }

        public static Task<IRestResponse> QueryAsync(Network network, string address)
        {
            var client = new RestClient(network.lcdUrl);
            var request = new RestRequest($"/auth/accounts/{address}", Method.GET);                        
            return client.ExecuteAsync(request);
        }
    }
}
