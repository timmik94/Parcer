using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Opera;
using Parcing.Models;
using Parcing.Parcers;
using Parcing.Proxies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Parcing
{
    class Program
    {

        public static ProxyConnector proxyConnector;
        public static FileJsonWriter jsonWriter;
     
        static void Main(string[] args)
        {
            BuildProxies();

         
            jsonWriter = new FileJsonWriter(100, "Rev");

            Console.WriteLine("Input baseURL");
            string URL = Console.ReadLine();

            Console.Write("Input FirstPage:");
            int first = int.Parse(Console.ReadLine());

            Console.Write("Input last page:");
            int last = int.Parse(Console.ReadLine());
          
            ParceItemsUrls parcer = new ParceItemsUrls(URL,last,first );
            parcer.ParceArea();


            //ParceItemsUrls parcer2 = new ParceItemsUrls("http://otzovik.com/health/medicines/", 402);
            //parcer.ParceArea();



            jsonWriter.WriteAll();

          
        }

        static void BuildProxies()
        {
            StreamReader sr = new StreamReader("ProxyList.txt");
            
            proxyConnector = new ProxyConnector(new List<ProxyVM>());
            while (!sr.EndOfStream)
            {
                string proxy = sr.ReadLine();
                string[] ipport = proxy.Split(':');
                ProxyVM vm = new ProxyVM()
                {
                    ip = ipport[0],
                    Port = int.Parse(ipport[1])
                };
                proxyConnector.AddProxy(vm);
            }

        }
    }
}
