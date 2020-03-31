using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class ModifyWithdrawAddress : Msg, IMsg
    {
        public ModifyWithdrawAddress() { }

        public ModifyWithdrawAddress(Network network, string delegator_address, string withdraw_address,
            string account_number = null, string sequence = null, string memo = "",
            string fee = null, string gas = null)
        {
            // required params
            this.msgType = MsgType.Modify_Withdraw_Address;
            this.network = network;
            this.address = delegator_address;

            // value params
            this.value = new ValueParams(delegator_address, withdraw_address);

            // optional variables
            this.account_number = account_number;
            this.sequence = sequence;
            this.memo = memo;
            this.fee = fee;
            this.gas = gas;
        }

        public class ValueParams : IMsgValue
        {
            public string delegator_address;
            public string withdraw_address;

            public ValueParams() { }

            public ValueParams(string delegator_address, string withdraw_address)
            {
                this.delegator_address = delegator_address;
                this.withdraw_address = withdraw_address;
            }
        }
    }
}
