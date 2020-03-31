using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class Send : Msg, IMsg
    {
        public Send() { }

        public Send(Network network, List<Coin> amount, string from_address, string to_address, 
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Send;
            this.network = network;                        
            this.address = from_address;

            // value params
            this.value = new ValueParams(amount, from_address, to_address);

            // optional variables
            this.account_number = account_number;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.gas = gas;
        }        

        public class ValueParams : IMsgValue
        {
            public List<Coin> amount;
            public string from_address;
            public string to_address;

            public ValueParams() { }

            public ValueParams(List<Coin> amount, string from_address, string to_address)
            {
                this.amount = amount;
                this.from_address = from_address;
                this.to_address = to_address;
            }
        }
    }
}
