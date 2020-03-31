using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public interface IMsg
    {
        MsgType msgType { get; set; }
        Network network { get; set; }        
        IMsgValue value { get; set; }
        List<TxMsg> msgs { get; set; }
        string address { get; set; }
        string account_number { get; set; }
        string sequence { get; set; }
        string memo { get; set; }
        string fee { get; set; }
        string gas { get; set; }
    }
}
