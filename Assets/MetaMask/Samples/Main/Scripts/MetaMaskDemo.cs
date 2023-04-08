using System;

using MetaMask.Models;
using Nethereum.Hex.HexTypes;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Web3Unity.Scripts.Library.Ethers.Transactions;

namespace MetaMask.Unity.Samples
{

    public class MetaMaskDemo : MonoBehaviour
    {
        private JsonRpcProvider _provider; 
        private void Start()
        {
            _provider = new JsonRpcProvider("https://goerli.infura.io/v3/b574497921744232a0c128d304243fd1");
        }

        #region Events

        /// <summary>Raised when the wallet is connected.</summary>
        public event EventHandler onWalletConnected;
        /// <summary>Raised when the wallet is disconnected.</summary>
        public event EventHandler onWalletDisconnected;
        /// <summary>Raised when the wallet is ready.</summary>
        public event EventHandler onWalletReady;
        /// <summary>Raised when the wallet is paused.</summary>
        public event EventHandler onWalletPaused;
        /// <summary>Raised when the user signs and sends the document.</summary>
        public event EventHandler onSignSend;
        /// <summary>Occurs when a transaction is sent.</summary>
        public event EventHandler onTransactionSent;
        /// <summary>Raised when the transaction result is received.</summary>
        /// <param name="e">The event arguments.</param>
        public event EventHandler<MetaMaskEthereumRequestResultEventArgs> onTransactionResult;

        #endregion

        #region Fields

        /// <summary>The configuration for the MetaMask client.</summary>
        [SerializeField]
        protected MetaMaskConfig config;

        #endregion

        #region Unity Methods

        /// <summary>Initializes the MetaMaskUnity instance.</summary>
        private void Awake()
        {
            MetaMaskUnity.Instance.Initialize();
            MetaMaskUnity.Instance.Wallet.WalletAuthorized += walletConnected;
            MetaMaskUnity.Instance.Wallet.WalletDisconnected += walletDisconnected;
            MetaMaskUnity.Instance.Wallet.WalletReady += walletReady;
            MetaMaskUnity.Instance.Wallet.WalletPaused += walletPaused;
            MetaMaskUnity.Instance.Wallet.EthereumRequestResultReceived += TransactionResult;
        }

        private void OnDisable()
        {
            MetaMaskUnity.Instance.Wallet.WalletAuthorized -= walletConnected;
            MetaMaskUnity.Instance.Wallet.WalletDisconnected -= walletDisconnected;
            MetaMaskUnity.Instance.Wallet.WalletReady -= walletReady;
            MetaMaskUnity.Instance.Wallet.WalletPaused -= walletPaused;
            MetaMaskUnity.Instance.Wallet.EthereumRequestResultReceived -= TransactionResult;
        }
        
        #endregion

        #region Event Handlers

        /// <summary>Raised when the transaction result is received.</summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        public void TransactionResult(object sender, MetaMaskEthereumRequestResultEventArgs e)
        {
            onTransactionResult?.Invoke(sender, e);
        }

        /// <summary>Raised when the wallet is connected.</summary>
        private void walletConnected(object sender, EventArgs e)
        {
            onWalletConnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raised when the wallet is disconnected.</summary>
        private void walletDisconnected(object sender, EventArgs e)
        {
            onWalletDisconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raised when the wallet is ready.</summary>
        private void walletReady(object sender, EventArgs e)
        {
            onWalletReady?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>Raised when the wallet is paused.</summary>
        private void walletPaused(object sender, EventArgs e)
        {
            onWalletPaused?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void OpenDeepLink() {
            if (MetaMask.Transports.Unity.UI.MetaMaskUnityUITransport.DefaultInstance != null)
            {
                MetaMask.Transports.Unity.UI.MetaMaskUnityUITransport.DefaultInstance.OpenDeepLink();
            }
        }

        /// <summary>Calls the connect method to the Metamask Wallet.</summary>
        public void Connect()
        {
            MetaMaskUnity.Instance.Wallet.Connect();
        }

        /// <summary>Sends a transaction to the Ethereum network.</summary>
        /// <param name="transactionParams">The parameters of the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async void SendTransaction()
        {
            var contractAddress = "0x8A1643ABfd117E27CB59E4406c05834f3b1f305E";
            
            var contractABI =  "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"approved\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeMint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"getApproved\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"ownerOf\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"tokenURI\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
            var gasPrice = await _provider.GetGasPrice();
            Debug.Log("Current Gas Price: " + gasPrice.Value);
            var contract = new Contract(contractABI, contractAddress, _provider);
            var calldata = contract.Calldata("safeMint", new[]
            {
                "0xE51995Cdb3b1c109E0e6E67ab5aB31CDdBB83E4a",
                "3"
            });
           
            Debug.Log("Contract Data: " + calldata);
            var transactionParams = new Transaction
            {
                To = contractAddress,
                From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
                Value = new HexBigInteger("0x0"),
                Data = calldata
            };
         
            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_sendTransaction",
                Parameters = new[] { transactionParams }
            };
            
            onTransactionSent?.Invoke(this, EventArgs.Empty);
            await MetaMaskUnity.Instance.Wallet.Request(request);

        }

        /// <summary>Signs a message with the user's private key.</summary>
        /// <param name="msgParams">The message to sign.</param>
        /// <exception cref="InvalidOperationException">Thrown when the application isn't in foreground.</exception>
        public async void Sign()
        {
            string msgParams = "{\"domain\":{\"chainId\":5,\"name\":\"Ether Mail\",\"verifyingContract\":\"0xCcCCccccCCCCcCCCCCCcCcCccCcCCCcCcccccccC\",\"version\":\"1\"},\"message\":{\"contents\":\"Hello, ChainSafe!\",\"from\":{\"name\":\"kwame.eth\",\"wallets\":[\"0xCD2a3d9F938E13CD947Ec05AbC7FE734Df8DD826\",\"0xDeaDbeefdEAdbeefdEadbEEFdeadbeEFdEaDbeeF\"]},\"to\":[{\"name\":\"kwame.eth\",\"wallets\":[\"0xbBbBBBBbbBBBbbbBbbBbbbbBBbBbbbbBbBbbBBbB\",\"0xB0BdaBea57B0BDABeA57b0bdABEA57b0BDabEa57\",\"0xB0B0b0b0b0b0B000000000000000000000000000\"]}]},\"primaryType\":\"Mail\",\"types\":{\"EIP712Domain\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"version\",\"type\":\"string\"},{\"name\":\"chainId\",\"type\":\"uint256\"},{\"name\":\"verifyingContract\",\"type\":\"address\"}],\"Group\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"members\",\"type\":\"Person[]\"}],\"Mail\":[{\"name\":\"from\",\"type\":\"Person\"},{\"name\":\"to\",\"type\":\"Person[]\"},{\"name\":\"contents\",\"type\":\"string\"}],\"Person\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"wallets\",\"type\":\"address[]\"}]}}";
            string from = MetaMaskUnity.Instance.Wallet.SelectedAddress;

            var paramsArray = new string[] { from, msgParams };

            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_signTypedData_v4",
                Parameters = paramsArray
            };
            onSignSend?.Invoke(this, EventArgs.Empty);
            await MetaMaskUnity.Instance.Wallet.Request(request);
        }
        #endregion
    }
}