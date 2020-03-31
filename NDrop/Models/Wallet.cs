using NBitcoin;
using NBitcoin.DataEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDrop
{
    public class Wallet
    {
        public Network Network;
        public string Mnemonic;
        public string HdPath;
        public byte[] PrivateKey;
        public string Address;        

        public Wallet() { }

        /// <summary>
        /// Creates a Wallet from provided network information and mnemonic phrase for use with signing and broadcasting transactions
        /// </summary>
        /// <param name="network">Network contains all relevant information for the connected network</param>        
        /// <param name="mnemonic">Mnemonic phrase to use for transaction signing</param>                
        /// <param name="hdPath">Optional: if not provided, will be set to network.defaultHdPath</param>                
        public Wallet(Network network, string mnemonic, string hdPath = null)
        {
            Network = network;
            Mnemonic = mnemonic;
            HdPath = string.IsNullOrEmpty(hdPath) ? network.defaultHdPath : hdPath;
            GetKeys();
        }

        /// <summary>
        /// Generates a new wallet
        /// </summary>
        /// <param name="network">Network contains all relevant information for the connected network</param> 
        /// <param name="hdPath">Optional: if not provided, will be set to network.defaultHdPath</param>      
        /// <returns></returns>
        public static Wallet Generate(Network network, string hdPath = null)
        {                        
            var mnemonic = new Mnemonic(Wordlist.English);
            if (string.IsNullOrEmpty(hdPath)) hdPath = network.defaultHdPath;

            return new Wallet(network, string.Join(" ", mnemonic.Words), hdPath);
        }

        private void GetKeys()
        {
            var key = new ExtKey(new Mnemonic(Mnemonic).DeriveSeed()).Derive(KeyPath.Parse(HdPath));
            this.PrivateKey = key.PrivateKey.ToBytes();
            this.Address = new Bech32(Network.bech32Prefix).Encode(key.PrivateKey.PubKey.Hash.ToBytes());            
        }

        /// <summary>
        /// Get available wallet balance
        /// </summary>
        /// <returns></returns>
        public Coin GetAvailableBalance()
        {
            var account = Account.Query(Network, Address);
            return account.coins.Any(c => c.denom == Network.denom) ? account.coins.Where(c => c.denom == Network.denom).ToList()[0] : Coin.Single("0", Network.denom);
        }

        /// <summary>
        /// Sign standard message
        /// </summary>
        /// <param name="stdMsg"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public SignedTx Sign(StdMsg stdMsg, SyncMode mode = SyncMode.Sync)
        {
            return new SignedTx(stdMsg, PrivateKey, mode);
        }

        /// <summary>
        /// Broadcast signed transaction and wait for response
        /// </summary>
        /// <param name="signedTx"></param>
        /// <returns></returns>
        public BroadcastResponse Broadcast(SignedTx signedTx)
        {
            Task<IRestResponse> task = Task.Run(async () => await BroadcastAsync(signedTx));
            return JObject.Parse(task.Result.Content).ToObject<BroadcastResponse>();
        }

        /// <summary>
        /// Broadcast signed transaction asynchronously
        /// </summary>
        /// <param name="signedTx"></param>
        /// <returns></returns>
        public Task<IRestResponse> BroadcastAsync(SignedTx signedTx)
        {
            var client = new RestClient(Network.lcdUrl);
            var request = new RestRequest("/txs", Method.POST);
            string jsonToSend = JsonConvert.SerializeObject(signedTx);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            return client.ExecuteAsync(request);
        }

        /// <summary>
        /// Creates a send msg
        /// </summary>
        /// <param name="amount">Amount in denom format to send</param>
        /// <param name="destination">Receiver address</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.Send Send(
            string amount, string destination, 
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.Send()
            {
                network = Network,
                msgType = MsgType.Send,
                value = new Msgs.Send.ValueParams()
                {
                    amount = Coin.SingleArr(amount, Network.denom),
                    from_address = Address,
                    to_address = destination
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,                
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a delegate msg
        /// </summary>
        /// <param name="amount">Amount to delegate to validator</param>
        /// <param name="validator_address">Validator address to delegate to</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.Delegate Delegate(
            string amount, string validator_address,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.Delegate()
            {                
                network = Network,
                msgType = MsgType.Delegate,
                value = new Msgs.Delegate.ValueParams()
                {
                    amount = Coin.Single(amount, Network.denom),
                    delegator_address = Address,
                    validator_address = validator_address
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates an undelegate msg
        /// </summary>
        /// <param name="amount">Amount to undelegate from validator</param>
        /// <param name="validator_address">Validator address to undelegate from</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.Undelegate Undelegate(
            string amount, string validator_address,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.Undelegate()
            {
                network = Network,
                msgType = MsgType.Undelegate,
                value = new Msgs.Undelegate.ValueParams()
                {
                    amount = Coin.Single(amount, Network.denom),
                    delegator_address = Address,
                    validator_address = validator_address
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a redelegate msg
        /// </summary>
        /// <param name="amount">Amount to transfer between validators</param>
        /// <param name="validator_src_address">Origin / source validator address</param>
        /// <param name="validator_dst_address">Destination validator address</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.Redelegate Redelegate(
            string amount, string validator_src_address, string validator_dst_address,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.Redelegate()
            {
                network = Network,
                msgType = MsgType.Begin_Redelegate,
                value = new Msgs.Redelegate.ValueParams()
                {
                    amount = Coin.Single(amount, Network.denom),
                    delegator_address = Address,
                    validator_src_address = validator_src_address,
                    validator_dst_address = validator_dst_address
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a withdraw rewards msg
        /// </summary>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>           
        /// <returns></returns>
        public Msgs.WithdrawRewards WithdrawRewards(
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            var client = new RestClient(Network.lcdUrl);
            var request = new RestRequest($"/distribution/delegators/{Address}/rewards", Method.GET);
            var response = client.Execute(request);
            var rewards = JObject.Parse(response.Content)["result"]["rewards"];

            List<TxMsg> msgs = new List<TxMsg>();
            foreach (JObject jObj in rewards)
                msgs.Add(new TxMsg(MsgType.Withdraw_Delegation_Reward, new Msgs.WithdrawRewards.ValueParams(Address, jObj["validator_address"].ToString())));

            return new Msgs.WithdrawRewards()
            {
                network = Network,
                msgType = MsgType.Withdraw_Delegation_Reward,
                msgs = msgs,
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a modify withdraw address msg
        /// </summary>
        /// <param name="withdraw_address">New destination withdraw address for withdraw rewards transactions</param>    
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>    
        /// <returns></returns>
        public Msgs.ModifyWithdrawAddress ModifyWithdrawAddress(
            string withdraw_address,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.ModifyWithdrawAddress()
            {
                network = Network,
                msgType = MsgType.Modify_Withdraw_Address,
                value = new Msgs.ModifyWithdrawAddress.ValueParams()
                {
                    delegator_address = Address,
                    withdraw_address = withdraw_address
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a submit proposal msg for governance proposals
        /// </summary>        
        /// <param name="title">Title of governance proposal</param>
        /// <param name="description">Description of governance proposal</param>
        /// <param name="initial_deposit">Initial deposit for governance proposal</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.SubmitProposal SubmitProposal(
            string title, string description, string initial_deposit,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.SubmitProposal()
            {
                network = Network,
                msgType = MsgType.Submit_Proposal,
                value = new Msgs.SubmitProposal.ValueParams()
                {
                    content = new Msgs.SubmitProposal.ValueParams.Content()
                    {
                        type = "cosmos-sdk/TextProposal",
                        value = new Msgs.SubmitProposal.ValueParams.Content.Value()
                        {
                            title = title,
                            description = description
                        }
                    },
                    initial_deposit = Coin.SingleArr(initial_deposit, Network.denom),
                    proposer = Address
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }

        /// <summary>
        /// Creates a vote msg for active governance proposals
        /// </summary>
        /// <param name="voteOption">Vote option</param>
        /// <param name="proposal_id">Proposal ID to cast vote on</param>
        /// <param name="account_number">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="sequence">Account number and sequence must be provided together; when provided, overrides obtaining acc_num & seq from LCD API</param>
        /// <param name="memo">Public memo for transaction</param>
        /// <param name="fee">Override default network fee for transaction</param>
        /// <param name="gas">Override default network gas for transaction</param>
        /// <returns></returns>
        public Msgs.Vote Vote(
            VoteOption voteOption, string proposal_id,
            string account_number = null, string sequence = null, string memo = null,
            string fee = null, string gas = null)
        {
            return new Msgs.Vote()
            {
                network = Network,
                msgType = MsgType.Vote,
                value = new Msgs.Vote.ValueParams()
                {
                    option = voteOption.GetOption(),
                    proposal_id = proposal_id,
                    voter = Address          
                },
                address = Address,
                account_number = account_number,
                sequence = sequence,
                memo = memo,
                fee = fee,
                gas = gas
            };
        }
    }
}
