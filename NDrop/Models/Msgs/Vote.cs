using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class Vote : Msg, IMsg
    {
        public Vote() { }

        public Vote(Network network, VoteOption voteOption, string proposal_id, string voter,
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Vote;
            this.network = network;
            this.address = voter;

            // value params
            this.value = new ValueParams(voteOption, proposal_id, voter);
            
            // optional variables
            this.account_number = account_number;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.gas = gas;
        }

        public class ValueParams : IMsgValue
        {
            public string option;
            public string proposal_id;
            public string voter;

            public ValueParams() { }

            public ValueParams(VoteOption voteOption, string proposal_id, string voter)
            {
                this.option = voteOption.GetOption();
                this.proposal_id = proposal_id;
                this.voter = voter;
            }
        }
    }
}
