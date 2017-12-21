using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Parcing.Proxies
{
    class ProxyVM
    {
        public string ip;
        public int Port;
        public bool usable = true;
        public long UseTime=0;


        public void DoReady()
        {
            
                if (DateTime.Now.Ticks - UseTime < 25000)
                {
                    Thread.Sleep((int)(DateTime.Now.Ticks - UseTime));
                }
           
            
        }

    }
}
