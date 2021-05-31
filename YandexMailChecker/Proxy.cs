using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMailChecker
{
    public class Proxy
    {
        protected string address { get; private set; }
        protected string port { get; private set; }

        public Proxy(string address, string port)
        {
            this.address = address;
            this.port = port;
        }
    }
}
