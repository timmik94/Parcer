using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Parcing.Proxies
{
    class ProxyConnector
    {
        public List<ProxyVM> proxies;
        int errors=0;
        private int currentproxy;
        private int iteration=0;
        public ProxyConnector() { }

        public ProxyConnector(List<ProxyVM> prox)
        {
            proxies = prox;
            currentproxy =0;
        }


        private ProxyVM GetNext()
        {
            if (errors >= proxies.Count)
            {
                foreach (var item in proxies)
                {
                    item.usable = true;
                    errors = 0;
                    iteration++;
                }
                currentproxy = 0;
            }
                if (currentproxy >= proxies.Count)
                {
                    currentproxy = 0;
                }
                var curr = proxies[currentproxy];

                currentproxy++;
                if (curr.usable&&iteration<=2||(curr.usable&&(iteration>2)&&(curr.UseTime!=0)))
                {
                    return curr;
                }
                else { return GetNext(); }
        }

        public void AddProxy(ProxyVM vm)
        {
            proxies.Add(vm);
        }

        public HtmlDocument Connect(string url)
        {
            bool f = false;
           
            ProxyVM curr = GetNext();
            do
            {
                try
                {
                    
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Timeout = 2000;
                    req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X x.y; rv:42.0) Gecko/20100101 Firefox/42.0";
                    //req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
                    req.Proxy = new  WebProxy("http://"+curr.ip+":"+curr.Port+"/");
                    req.Accept = "text/html";
                    req.KeepAlive = false;
                    req.MaximumAutomaticRedirections = 50;
                    //req.
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    var doc = new HtmlDocument();
                    doc.Load(resp.GetResponseStream());
                   
                    resp.Close();
                    curr.UseTime = DateTime.Now.Ticks;
                    Console.WriteLine("loaded");
                    //Thread.Sleep(1000);
                    return doc;
                }
                catch(WebException e) { Console.WriteLine(e.Message+errors.ToString()+":"+currentproxy.ToString()); curr.usable = false; errors++; /*Console.WriteLine(errors);*/ }
                curr = GetNext();
                if (errors > proxies.Count) { break; }
                //catch(Exception e1) { if (e1.Message == "ProxiesEnds") { Console.WriteLine("Stop"); break; } }
            } while (!f);
            return null;
        }

    }
}
