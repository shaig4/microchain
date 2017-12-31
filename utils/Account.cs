
using System.Collections.Generic;

namespace utils
{
    public class Address
    {
        public string privateKey;
        public string publicKey;
        public string name;
        public string pubHash;
        public decimal balance;
    }
    public class Wallet
    {
        public List< Address> addresses=new List<Address>();
        public List<Coin> coins=new List<Coin>();
        public string name;
        public string AddKey(string name)
        {
            var a = CryptoUtils.CreateAddress();
            a.name = name;
            a.pubHash = CryptoUtils.HashAscii(a.publicKey);
            addresses.Add(a);
            return a.pubHash;
        }
        public Address Import(string publicKey, string privateKey)
        {
            var a = new Address();
            a.name = "imported";
            a.publicKey = publicKey;
            a.privateKey = privateKey;
            a.pubHash = CryptoUtils.HashAscii(publicKey);
                addresses.Add(a);
            return a;
        }
        public string Sign(Coin coin, Address a)
        {
            return  CryptoUtils.Sign(a.privateKey, coin.dataHash);
        }
        public Wallet(string _name)
        {
            name = _name;
        }
    }

}
