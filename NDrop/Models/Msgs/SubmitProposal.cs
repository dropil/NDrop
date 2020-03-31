using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class SubmitProposal : Msg, IMsg
    {
        public SubmitProposal() { }

        public SubmitProposal(Network network, string title, string description, List<Coin> initial_deposit, string proposer,
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Submit_Proposal;
            this.network = network;
            this.address = proposer;

            // value params
            this.value = new ValueParams(title, description, initial_deposit, proposer);            

            // optional variables
            this.account_number = account_number;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.gas = gas;
        }

        public class ValueParams : IMsgValue
        {
            public Content content;
            public List<Coin> initial_deposit;
            public string proposer;

            public ValueParams() { }

            public ValueParams(string title, string description, List<Coin> initial_deposit, string proposer)
            {
                content = new Content("cosmos-sdk/TextProposal", title, description);                
                this.initial_deposit = initial_deposit;
                this.proposer = proposer;
            }

            public class Content
            {
                public string type;
                public Value value;

                public Content() { }

                public Content(string type, string title, string description)
                {
                    this.type = type;
                    value = new Value(title, description);
                }

                public class Value
                {
                    public string title;
                    public string description;

                    public Value() { }

                    public Value(string title, string description)
                    {
                        this.title = title;
                        this.description = description;
                    }
                }
            }
        }
    }
}
