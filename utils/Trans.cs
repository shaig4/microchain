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
        //public string[] brothers;
        //public string publicKey;
        public string dataHash;
        public decimal amount;
        public string data;
        public DateTime time;
       // public bool available;

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
        public int from= -1;
        public List<Vote> votes;
    }
    public class Vote
    {
        public bool ok;
        public int from;
    }
    [Serializable]
    public class RequestChild
    {
        public string data;
        public decimal amount;
        public string pubHash;
    }
    [Serializable]
    public class RequestParent
    {
        public string sig;
        public string unlocker;
        public string pubHash;

        public string publicKey { get;  set; }
    }
}
