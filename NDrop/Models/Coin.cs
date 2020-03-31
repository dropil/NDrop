using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class Coin
    {
        public string amount;
        public string denom;

        public Coin() { }

        public Coin(string amount, string denom)
        {
            this.amount = amount;
            this.denom = denom;
        }

        public static List<Coin> SingleArr(string amount, string denom)
        {
            return new List<Coin>()
            {
                new Coin(amount, denom)
            };
        }

        public static Coin Single(string amount, string denom)
        {
            return new Coin(amount, denom);
        }
    }
}
