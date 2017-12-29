using blockchain1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace blockchain2
{
    class Program
    {
        Queue<string> queue = new Queue<string>();
        static void Main(string[] args)
        {
            var g = new Program();
            g.Init();
        }
        public void Init()
        { 
            var net = new Network();
            var gensysWallet = new Wallet("genesis");

            var f = File.ReadAllLines(@"C:\projects\blockchainUtils\imp.txt");
            gensysWallet.Import(f[0], f[1]);

            CryptoUtils.Pay(net, new RequestParent { },
                 new RequestChild { amount = 100, publicKey = gensysWallet.pubPriv.First().Key }, true);
                    Console.WriteLine("genesis:");
            Console.WriteLine(gensysWallet.pubPriv.First().Key);
            Console.WriteLine(gensysWallet.pubPriv.First().Value.privateKey);
            Console.WriteLine("---");

            var mini = new MiniServer();
            mini.Listen("http://localhost:8090/", queue,net);
            while (true)
            {
                if (queue.Count!=0)
                {
                    try
                    {
                        var x = queue.Dequeue();
                        Console.WriteLine("Payment Entered!");
                        Console.WriteLine(x);
                        var rp = JsonConvert.DeserializeObject<RequestPay>(x);
                        CryptoUtils.Pay(net, rp.p, rp.c, false);
                        Console.WriteLine("Payed!");


                        foreach (var a in net.coins)
                        {
                            Console.WriteLine(a.Value.amount + ":" + a.Value.available);
                        }
                    } catch
                    {
                        Console.WriteLine("Declined!");

                        foreach (var a in net.coins)
                        {
                            Console.WriteLine(a.Value.amount + ":"+a.Value.available);
                        }
                    }
                }
                Thread.Sleep(100);
            }

        }
    }
}
