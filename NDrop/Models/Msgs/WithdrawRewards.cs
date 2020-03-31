using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class WithdrawRewards : Msg, IMsg
    {
        public WithdrawRewards() { }       

        public class ValueParams : IMsgValue
        {
            public string delegator_address;
            public string validator_address;

            public ValueParams() { }

            public ValueParams(string delegator_address, string validator_address)
            {
                this.delegator_address = delegator_address;
                this.validator_address = validator_address;
            }
        }
    }
}
