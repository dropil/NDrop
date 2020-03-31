using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class TxMsg
    {
        public string type;
        public IMsgValue value;

        public TxMsg() { }

        public TxMsg(MsgType msgType, IMsgValue value)
        {
            this.type = msgType.GetMsgType();
            this.value = value;
        }
    }
}