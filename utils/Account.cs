
using System.Collections.Generic;

namespace utils
{
    public class Address
    {
        public string privateKey;
        public string publicKey;
        public string name;
        public decimal balance;
    }
    public class Wallet
    {
        public Dictionary<string, Address> pubPriv=new Dictionary<string, Address>();
        public List<Coin> coins=new List<Coin>();
        public string name;
        public string AddKey(string name)
        {
            var a = CryptoUtils.CreateAddress();
            a.name = name;
            pubPriv.Add(a.publicKey, a);
            return a.publicKey;
        }
        public void Import(string publicKey, string privateKey)
        {
            var a = new Address();
            a.name = "imported";
            a.publicKey = publicKey;
            a.privateKey = privateKey;
            if (!pubPriv.ContainsKey(publicKey))
                pubPriv.Add(publicKey, a);
        }
        public string Sign(Coin coin)
        {
            var a = pubPriv[coin.publicKey];
            return  CryptoUtils.Sign(a.privateKey, coin.hash);
        }
        public Wallet(string _name)
        {
            name = _name;
        }
    }

}
