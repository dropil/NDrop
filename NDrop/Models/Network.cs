using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NDrop
{    
    public class Network
    {
        public string chainId;
        public string lcdUrl;
        public string defaultHdPath;
        public string bech32Prefix;
        public string denom;
        public string powerReduction;
        public string defaultFee;
        public string defaultGas;

        /// <summary>
        /// Returns a new Network by providing all required parameters
        /// </summary>
        /// <param name="chainId">Network chain-id</param>
        /// <param name="lcdUrl">LCD API URL for broadcasting signed transactions and querying blockchain (may be localhost)</param>
        /// <param name="defaultHdPath">HD derivation path for obtaining wallet address</param>
        /// <param name="bech32Prefix">Prefix to append to wallet address (i.e. "drop" / "atom")</param>
        /// <param name="denom">Lower denomination for chain's native coin (i.e. "udrop" / "uatom")</param>
        /// <param name="powerReduction">Power reduction for number of decimals (i.e. 6 decimals = 1000000 power reduction)</param>
        /// <param name="baseFee">Default fee to use for transactions</param>
        /// <param name="baseGas">Default gas to use for transactions</param>
        public Network(string chainId, string lcdUrl, string defaultHdPath, string bech32Prefix, string denom, string powerReduction, string baseFee, string baseGas)
        {
            this.chainId = chainId;
            this.lcdUrl = lcdUrl;
            this.defaultHdPath = defaultHdPath;
            this.bech32Prefix = bech32Prefix;
            this.denom = denom;
            this.powerReduction = powerReduction;
            this.defaultFee = baseFee;
            this.defaultGas = baseGas;
        }

        /// <summary>
        /// Returns a new Network using provided chainId
        /// </summary>
        /// <param name="chainId">Network chain-id</param>
        /// <param name="lcdUrl">Optional: LCD API URL for broadcasting signed transactions and querying blockchain (may be localhost); defaults to network's publicly avilable LCD API</param>
        /// <param name="baseFee">Optional: provide this to override the default fee to use for transactions</param>
        /// <param name="baseGas">Optional: provide this to override the default gas to use for transactions</param>
        public Network(string chainId, string lcdUrl = null, string baseFee = null, string baseGas = null)
        {
            switch (chainId)
            {
                case "Dropil-Chain-Zeus":
                    throw new NotImplementedException($"Network '{chainId}' has not been implemented yet");

                case "Dropil-Chain-Poseidon":
                    this.chainId = "Dropil-Chain-Poseidon";
                    this.lcdUrl = string.IsNullOrEmpty(lcdUrl) ? "https://testnet-api.dropilchain.com" : lcdUrl;
                    defaultHdPath = "m/44'/495'/0'/0/0";
                    bech32Prefix = "drop";
                    denom = "udrop";
                    powerReduction = "1000000";
                    this.defaultFee = string.IsNullOrEmpty(baseFee) ? "10000" : baseFee;
                    this.defaultGas = string.IsNullOrEmpty(baseGas) ? "200000" : baseGas;

                    break;
                case "cosmoshub-3":
                    this.chainId = "cosmoshub-3";                    
                    this.lcdUrl = string.IsNullOrEmpty(lcdUrl) ? "https://api.cosmos.network" : lcdUrl;
                    defaultHdPath = "m/44'/118'/0'/0/0";
                    bech32Prefix = "cosmos";
                    denom = "uatom";
                    powerReduction = "1000000";
                    this.defaultFee = string.IsNullOrEmpty(baseFee) ? "5000" : baseFee;
                    this.defaultGas = string.IsNullOrEmpty(baseGas) ? "200000" : baseGas;

                    break;
                default:
                    throw new NotSupportedException($"Network for chain id '{chainId}' is not supported");
            }
        }

        /// <summary>
        /// Returns a new Network using provided NetworkName enum
        /// </summary>
        /// <param name="networkName"></param>
        /// <param name="lcdUrl">Optional: LCD API URL for broadcasting signed transactions and querying blockchain (may be localhost); defaults to network's publicly avilable LCD API</param>
        /// <param name="baseFee">Optional: provide this to override the default fee to use for transactions</param>
        /// <param name="baseGas">Optional: provide this to override the default gas to use for transactions</param>
        public Network(NetworkName networkName, string lcdUrl = null, string baseFee = null, string baseGas = null) : this(networkName.GetChainId(), lcdUrl, baseFee, baseGas) { }

        /// <summary>
        /// Returns a new Network using provided NetworkChainId enum
        /// </summary>        
        /// <param name="networkChainId"></param>
        /// <param name="lcdUrl">Optional: LCD API URL for broadcasting signed transactions and querying blockchain (may be localhost); defaults to network's publicly avilable LCD API</param>
        /// <param name="baseFee">Optional: provide this to override the default fee to use for transactions</param>
        /// <param name="baseGas">Optional: provide this to override the default gas to use for transactions</param>
        public Network(NetworkChainId networkChainId, string lcdUrl = null, string baseFee = null, string baseGas = null) : this(networkChainId.GetChainId(), lcdUrl, baseFee, baseGas) { }
    }
}
