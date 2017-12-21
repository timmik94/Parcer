using OpenQA.Selenium;
using OpenQA.Selenium.Opera;
using Parcing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parcing.Parcers
{
    class ParceItemsUrls
    {
        
        public string mainURL;
        public int currpage;
        public int maxpage;


        public ParceItemsUrls(string url,int lastpage,int firstpage=1)
        {
          
            mainURL = url;
            maxpage = lastpage;
            currpage = firstpage;
        }


       public void PArcePage(int pagenum)
       {

            try
            {
                string fullURL = mainURL + $"{pagenum}/";
                var doc = Program.proxyConnector.Connect(fullURL);

                var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"main_content\"]/div[4]/table/tr/td[2]/h3/a");

                foreach (var node in nodes)
                {
                    string itemurl = node.GetAttributeValue("href", "NOT_HREF");
                    if (Uri.IsWellFormedUriString(itemurl, UriKind.Absolute))
                    {

                        ParceReviewUrls parceRevURL = new ParceReviewUrls();
                        parceRevURL.GetRevUrls(itemurl);
                    }
                }
            }
            catch { Console.WriteLine("Incorrect format.");PArcePage(pagenum); }
        
        }

        public void ParceArea()
        {
           
            for(int curr = currpage; curr <= maxpage; curr++)
            {
                try
                {
                    PArcePage(currpage);
                }
                catch { Console.WriteLine("Page is missing"); }
            }
            Console.WriteLine("ParcedAll");
        } 

    }
}
