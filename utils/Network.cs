using System;
using System.Collections.Generic;
using System.Text;

namespace utils
{
    public class Network
    {
        public Dictionary<string,Coin> all = new Dictionary<string, Coin> ();
        public Dictionary<string, Coin> avail= new Dictionary<string, Coin> ();
        public string name;
    }

}
