using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class Redelegate : Msg, IMsg
    {
        public Redelegate() { }

        public Redelegate(Network network, Coin amount, string delegator_address, string validator_src_address, string validator_dst_address,
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Begin_Redelegate;
            this.network = network;
            this.address = delegator_address;

            // value params
            this.value = new ValueParams(amount, delegator_address, validator_src_address, validator_dst_address);

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
            public string validator_src_address;
            public string validator_dst_address;

            public ValueParams() { }

            public ValueParams(Coin amount, string delegator_address, string validator_src_address, string validator_dst_address)
            {
                this.amount = amount;
                this.delegator_address = delegator_address;
                this.validator_src_address = validator_src_address;
                this.validator_dst_address = validator_dst_address;
            }
        }
    }
}
