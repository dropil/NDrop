using Cryptography.ECDSA;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class PubKey
    {
        public string type;
        public string value;

        public PubKey() { }

        public PubKey(byte[] privateKey)
        {
            type = "tendermint/PubKeySecp256k1";
            value = Convert.ToBase64String(Secp256K1Manager.GetPublicKey(privateKey, true));
        }        
    }
}
