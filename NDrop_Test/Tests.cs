using System;
using NDrop;

namespace NDrop_Test
{
    class Tests
    {
        static void Main(string[] args)
        {
            var dropilNetwork = new Network(NetworkName.Dropil_Chain_Testnet);
            var cosmosNetwork = new Network(NetworkName.CosmosHub_Mainnet);

            var dropilWallet = Wallet.Generate(dropilNetwork);
            var cosmosWallet = Wallet.Generate(cosmosNetwork);

            // tests will be added soon! :)
        }
    }
}
