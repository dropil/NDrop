using System;
using System.Text;

namespace NDrop
{
    public class SignedTx
    {
        public Tx tx;
        public string mode;

        public SignedTx(StdMsg stdMsg, byte[] privateKey, SyncMode mode, string memo = "")
        {
            tx = new Tx(stdMsg, privateKey, memo);
            this.mode = mode.GetSyncMode();
        }
    }
}
