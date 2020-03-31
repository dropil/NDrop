using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class Delegate : Msg, IMsg
    {
        public Delegate() { }

        public Delegate(Network network, Coin amount, string delegator_address, string validator_address,
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Delegate;
            this.network = network;
            this.address = delegator_address;

            // value params
            this.value = new ValueParams(amount, delegator_address, validator_address);

            // optional variables
            this.account_number = account_number;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.gas = gas;
        }

        public class ValueParams : IMsgValue
        {
            public Coin amount;
            public string delegator_address;
            public string validator_address;

            public ValueParams() { }

            public ValueParams(Coin amount, string delegator_address, string validator_address)
            {
                this.amount = amount;
                this.delegator_address = delegator_address;
                this.validator_address = validator_address;
            }
        }
    }
}
