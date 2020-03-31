using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class Tx
    {
        public List<TxMsg> msg;
        public Fee fee;
        public List<Signature> signatures;
        public string memo;

        public Tx(StdMsg stdMsg, byte[] privateKey, string memo = "")
        {
            msg = stdMsg.input.msgs;
            fee = stdMsg.input.fee;
            signatures = new List<Signature>()
                {
                    new Signature(stdMsg, privateKey)
                };
            this.memo = memo;
        }
    }
}
