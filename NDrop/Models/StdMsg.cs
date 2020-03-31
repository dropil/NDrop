using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDrop
{
    public class StdMsg
    {
        public StdMsgInput input;
        public byte[] bytes;

        public StdMsg(StdMsgInput input)
        {
            this.input = input;           

            var jObj = Utils.JSON.Sort(JObject.FromObject(input));
            bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jObj));
        }        

        

        public static StdMsg Build(IMsg msg)
        {
            msg.Validate();

            return new StdMsg(new StdMsgInput() {
                msgs = new List<TxMsg>() { 
                    new TxMsg(msg.msgType, msg.value) 
                },
                chain_id = msg.network.chainId,
                fee = new Fee(Coin.SingleArr(msg.fee, msg.network.denom), msg.gas),
                account_number = msg.account_number,
                sequence = msg.sequence,
                memo = msg.memo
            });
        }

        public static StdMsg Build(List<TxMsg> msgs, IMsg msg)
        {
            msg.Validate();

            return new StdMsg(new StdMsgInput()
            {
                msgs = msgs,
                chain_id = msg.network.chainId,
                fee = new Fee(Coin.SingleArr(msg.fee, msg.network.denom), msg.gas),
                account_number = msg.account_number,
                sequence = msg.sequence,
                memo = msg.memo
            });
        }

        public class StdMsgInput
        {
            public List<TxMsg> msgs;
            public string chain_id;
            public Fee fee;
            public string memo;
            public string account_number;
            public string sequence;
        }
    }
}
