using utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {

            var genesisWallet = new Wallet($"genesis");
            var inbox = new Queue<string>();
            genesisWallet.AddKey("genesis");
            var addr = genesisWallet.pubPriv.First().Value;
            File.WriteAllLines(@"..\wallet\imp.txt", new string[] { addr.publicKey, addr.privateKey });


            for (var i = 0; i < 10; i++)
            {
                var g = new Program();
                g.InitBackground(i);
            }
            Console.ReadLine();
        }
        public void InitBackground(int i)
        {
            Task.Run(() =>
            {
                Init(i);
            });
        }
        public void Init(int i)
        {
            var net = new Network();
            var genesisWallet = new Wallet($"genesis");
            var f = File.ReadAllLines(@"..\wallet\imp.txt");
            genesisWallet.Import(f[0], f[1]);
            var addr = genesisWallet.pubPriv.First().Value;

            var inbox = new Queue<string>();

            CryptoUtils.Pay(net, null,
                 new RequestChild[] { new RequestChild { amount = 100, publicKey = addr.publicKey } });

            Console.WriteLine($"{i} genesis created ");
            //Console.WriteLine(genesisWallet.pubPriv.First().Key);
            //Console.WriteLine(genesisWallet.pubPriv.First().Value.privateKey);
            //Console.WriteLine($"---");

            var mini = new MiniServer();
            mini.Listen($"http://localhost:809{i}/", inbox, net);
            var voting = new Dictionary<string, RequestPay>();

            while (true)
            {
                if (inbox.Count != 0)
                {
                    var x = inbox.Dequeue();
                    Console.WriteLine($"{i} Payment Entered!");
                    var rp = JsonConvert.DeserializeObject<RequestPay>(x);

                    var hash = CryptoUtils.HashObj(rp.p);
                    if (voting.ContainsKey(hash))
                    {
                        //informed vote
                        rp = voting[hash];
                        rp.votes.Add(rp.valid);

                        if (rp.votes.Count == 10)
                        {

                            if (rp.votes.Where(v => v).Count() > rp.votes.Count / 2)
                            {
                                Console.WriteLine($"{i} COMEETE APPROVED");
                                CryptoUtils.Pay(net, rp.p, rp.c);
                            }
                            else
                                Console.WriteLine($"{i} COMEETE DECLINED");
                        }
                        Console.WriteLine($"{i} COMEETE {rp.votes.Count} count");
                        continue;
                    }
                    rp.votes = new List<bool> ();

                    if (rp.echo)
                        rp.votes.Add(rp.valid );

                    voting.Add(hash, rp);

                    try
                    {
                        CryptoUtils.Validate(net, rp.p, rp.c);
                        rp.valid = true;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"{i} invalid! " + ex.Message);
                        rp.valid = false;
                    }
                    if (i < 3)
                        rp.valid = !rp.valid;

                    rp.votes.Add(rp.valid );
                    Console.WriteLine($"{i} COMEETE {rp.votes.Count} count");
                    rp.echo = true;
                    Console.WriteLine($"{i} inform commitee {rp.valid}");
                    for (var j = 0; j < 10; j++)
                        if (j != i)
                        {
                            //send to other node
                            ApiUtils.Send(rp, j);
                        }

                    //   Console.WriteLine(x);
                    //foreach (var a in net.coins)
                    //{
                    //    Console.WriteLine(a.Value.amount + ":" + a.Value.available);
                    //}
                }
                Thread.Sleep(100);
            }

        }
    }
}
