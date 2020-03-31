using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Cryptography.ECDSA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NDrop
{
    public class Signature
    {
        public PubKey pub_key;
        public string signature;

        public Signature(StdMsg stdMsg, byte[] privateKey)
        {
            pub_key = new PubKey(privateKey);

            var stdMsgJObj = JObject.FromObject(stdMsg.input);
            var stdMsgJObjSorted = Utils.JSON.Sort(stdMsgJObj);
            var messageJSON = JsonConvert.SerializeObject(stdMsgJObjSorted);
            var messageBytes = Encoding.UTF8.GetBytes(messageJSON);
            var messageHashBytes = SHA256.Create().ComputeHash(messageBytes);
            var signBytes = Secp256K1Manager.SignCompact(messageHashBytes, privateKey, out _);

            signature = Convert.ToBase64String(signBytes);
        }
    }
}
