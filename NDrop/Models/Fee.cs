using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class Fee
    {
        public List<Coin> amount;
        public string gas;

        public Fee(List<Coin> amount, string gas)
        {
            this.amount = amount;
            this.gas = gas;
        }        
    }
}
