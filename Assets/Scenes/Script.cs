using NBitcoin;
using UnityEngine;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Nethereum.HdWallet;
using Newtonsoft.Json;

public class Script : MonoBehaviour
{
    // Start is called before the first frame update
    private JsonRpcProvider _rpc;
    public async void  Start()
    {
        _rpc = new JsonRpcProvider("https://goerli.infura.io/v3/ADD_API_KEY");
        var block = await _rpc.GetBlock();
        Debug.Log("Block Data: " + JsonConvert.SerializeObject(block,Formatting.Indented));
        Wallet wallet = new Wallet(Wordlist.English, WordCount.Twelve);
        Debug.Log("HD Wallet Path: " + wallet.Path);
        Debug.Log("HD Public Address: " + wallet.GetAccount(0).Address);
        Debug.Log("HD Private Key: " + wallet.GetAccount(0).PrivateKey);
        Debug.Log("HD Account Public Key: " + wallet.GetAccount(0).PublicKey);
    }
}
