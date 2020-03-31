using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NDrop
{
    public enum MsgType
    {
        [Description("cosmos-sdk/MsgSend")]
        Send,

        [Description("cosmos-sdk/MsgMultiSend")]
        Multi_Send,

        [Description("cosmos-sdk/MsgCreateValidator")]
        Create_Validator,

        [Description("cosmos-sdk/MsgEditValidator")]
        Edit_Validator,

        [Description("cosmos-sdk/MsgDelegate")]
        Delegate,

        [Description("cosmos-sdk/MsgUndelegate")]
        Undelegate,

        [Description("cosmos-sdk/MsgBeginRedelegate")]
        Begin_Redelegate,

        [Description("cosmos-sdk/MsgWithdrawDelegationReward")]
        Withdraw_Delegation_Reward,

        [Description("cosmos-sdk/MsgWithdrawValidatorCommission")]
        Withdraw_Validator_Commission,

        [Description("cosmos-sdk/MsgModifyWithdrawAddress")]
        Modify_Withdraw_Address,

        [Description("cosmos-sdk/MsgSubmitProposal")]
        Submit_Proposal,

        [Description("cosmos-sdk/MsgDeposit")]
        Deposit,

        [Description("cosmos-sdk/MsgVote")]
        Vote,

        [Description("cosmos-sdk/MsgUnjail")]
        Unjail
    }

    public enum NetworkName
    {
        [Description("Dropil-Chain-Zeus")]
        Dropil_Chain_Mainnet,

        [Description("Dropil-Chain-Poseidon")]
        Dropil_Chain_Testnet,

        [Description("cosmoshub-3")]
        CosmosHub_Mainnet
    }


    public enum NetworkChainId
    {
        [Description("Dropil-Chain-Zeus")]
        Dropil_Chain_Zeus,

        [Description("Dropil-Chain-Poseidon")]
        Dropil_Chain_Poseidon,

        [Description("cosmoshub-3")]
        cosmoshub_3
    }

    public enum SyncMode
    {
        [Description("sync")]
        Sync,

        [Description("async")]
        Async,

        [Description("block")]
        Block
    }

    public enum VoteOption
    {
        [Description("Yes")]
        Yes,

        [Description("No")]
        No,

        [Description("NoWithVeto")]
        NoWithVeto,

        [Description("Abstain")]
        Abstain
    }
}
