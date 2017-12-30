using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace utils
{
    [DebuggerDisplay("{this.ToString()}")]
    [Serializable]
    public class Coin
    {
        public string[] parents;
        public string[] brothers;
        public string publicKey;
        public string hash;
        public decimal amount;
        public string data;
        public DateTime time;
        public bool available;

        public override string ToString()
        {
            return $"amount: {amount}!!!!";
        }
    }
    [Serializable]
    public class RequestPay
    {
        public RequestParent[] p;
        public RequestChild[] c;
        public bool valid = true;
        public bool echo = false;
        public List<bool> votes;
    }
    [Serializable]
    public class RequestChild
    {
        public string data;
        public decimal amount;
        public string publicKey;
    }
    [Serializable]
    public class RequestParent
    {
        public string sig;
        public string unlocker;
        public string publicKey;
    }
}
