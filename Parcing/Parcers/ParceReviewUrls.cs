using Parcing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Parcing.Parcers
{
    class ParceReviewUrls
    {
        RevContext context;

        public ParceReviewUrls()
        {
           
        }

        public void GetRevUrls(string itemURL)
        {
            try
            {
                var doc = Program.proxyConnector.Connect(itemURL);
                var nodes = doc.DocumentNode.SelectNodes("//*[@class=\"summary \"]/div/meta[1]");///div/div/div/div[2]");//"]");//div");///div[3]/div[1]");//div[3]");//div/meta[1]");
                // doc.Save(File.Create("htmlFile.html"));
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        string revURL = node.GetAttributeValue("content", "NOT_URL");
                        ParceReview parceRev = new ParceReview();
                        parceRev.Parce(revURL);
                    }
                }
            }catch(Exception e) { Console.WriteLine("Invalid item page:" + e.Message); }
        }


    }
}
