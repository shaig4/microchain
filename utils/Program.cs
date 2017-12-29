using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace blockchain1
{
    class Program
    {
        static void Main(string[] args)
        {
            var gensysWallet = new Wallet("genesis");
            var secWallet = new Wallet("secWallet");
            var aliceWallet = new Wallet("alice");
            var bobWallet = new Wallet("bob");
            var net = new Network();

            var genesisCoin = CryptoUtils.Pay(net,
             new RequestParent { },
            new RequestChild { amount = 100, publicKey = gensysWallet.AddKey() }, genesis: true);

            gensysWallet.coins.Add(genesisCoin);

            var start = DateTime.Now;
            var a23Coins= CryptoUtils.PaySplit(net,
                new RequestParent{ publicKey= genesisCoin.publicKey, sig = gensysWallet.Sign(genesisCoin) }
            , new RequestChild[] {
                new RequestChild { amount = 10, publicKey = aliceWallet.AddKey()},
                new RequestChild { amount = 90, publicKey = bobWallet.AddKey() },
            });

            var a4 = CryptoUtils.CreateAddress();
            CryptoUtils.PayUnion(net, new RequestParent[] {
                new RequestParent{ publicKey = aliceWallet.pubPriv.First().Key, sig=CryptoUtils.Sign(aliceWallet.pubPriv.First().Value, a23Coins[0].hash) },
                new RequestParent{ publicKey = bobWallet.pubPriv.First().Key, sig=CryptoUtils.Sign(bobWallet.pubPriv.First().Value, a23Coins[1].hash) }
            },
                new RequestChild { amount = 100, publicKey = a4.publicKey }
            );
            var ms = DateTime.Now.Subtract(start).TotalMilliseconds;
           
        }

     }
}