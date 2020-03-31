using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace NDrop
{
    public static class Extensions
    {
        public static string GetMsgType(this MsgType value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetSyncMode(this SyncMode value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetChainId(this NetworkName value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetChainId(this NetworkChainId value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetOption(this VoteOption value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static void Validate(this IMsg msg)
        {
            // handle missing account number and/or sequence fields by querying blockchain for up-to-date values
            if (string.IsNullOrEmpty(msg.account_number) || string.IsNullOrEmpty(msg.sequence))
            {
                var account = Account.Query(msg.network, msg.address);
                msg.account_number = account.account_number;
                msg.sequence = account.sequence;
            }

            // handle missing fee and/or gas
            if (string.IsNullOrEmpty(msg.fee)) msg.fee = msg.network.defaultFee;
            if (string.IsNullOrEmpty(msg.gas)) msg.gas = msg.network.defaultGas;

            // set null memo to empty string
            if (string.IsNullOrEmpty(msg.memo)) msg.memo = "";
        }

        /// <summary>
        /// Builds a standard message from a msg that implements the IMsgParams interface
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static StdMsg BuildStdMsg(this IMsg msg)
        {
            if (msg.msgs != null) return StdMsg.Build(msg.msgs, msg);
            return StdMsg.Build(msg);
        }

        /// <summary>
        /// Signs a standard message using the provided wallet
        /// </summary>
        /// <param name="stdMsg"></param>
        /// <param name="wallet"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static SignedTx Sign(this StdMsg stdMsg, Wallet wallet, SyncMode mode = SyncMode.Sync)
        {
            return wallet.Sign(stdMsg, mode);
        }

        /// <summary>
        /// Broadcasts a signed transaction using the provided wallet and waits for response
        /// </summary>
        /// <param name="signedTx"></param>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public static BroadcastResponse Broadcast(this SignedTx signedTx, Wallet wallet)
        {
            return wallet.Broadcast(signedTx);
        }

        /// <summary>
        /// Broadcasts a signed transaction using the provided wallet asynchronously
        /// </summary>
        /// <param name="signedTx"></param>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public static Task<IRestResponse> BroadcastAsync(this SignedTx signedTx, Wallet wallet)
        {
            return wallet.BroadcastAsync(signedTx);
        }
    }
}
