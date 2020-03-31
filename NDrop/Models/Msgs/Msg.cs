using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop.Msgs
{
    public class Msg : IMsg
    {
        public MsgType msgType { get; set; }
        public Network network { get; set; }        
        public IMsgValue value { get; set; }
        public List<TxMsg> msgs { get; set; } = null;
        public string address { get; set; }
        public string account_number { get; set; } = null;
        public string sequence { get; set; } = null;
        public string memo { get; set; } = "";
        public string fee { get; set; } = null;
        public string gas { get; set; } = null;
    }
}
