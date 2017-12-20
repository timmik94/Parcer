using HtmlAgilityPack;
using Parcing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Parcing.Parcers
{
    class ParceReview
    {
        RevContext context;
        const float barlength= 68;


        public ParceReview()
        {
           // context = _context;
        }


        string BuildPathString(HtmlDocument doc)
        {
            var pathNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"main_content\"]/div/div/div/div/div[1]/div[1]");
            string s = "";
            for (int i = 0; i < 4; i++)
            {
                var node = pathNode.ChildNodes[i];
                s = s + node.InnerText + ">";
            }
            s += pathNode.LastChild.FirstChild.InnerText;
            return s;
        }

        //Div with data about user and review rating
        void BuildRevData(HtmlDocument doc,ref Review review)
        {
            User user = new User();
            var userDataNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"main_head_user_login\"]");
           
            user.Name = userDataNode.SelectSingleNode("//*[@id=\"author\"]").InnerText;
            user.From = userDataNode.LastChild.InnerText;

            var userRateNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"main_head_user_karma\"]");
            user.Reputation = int.Parse( userRateNode.SelectSingleNode("//i[1]").InnerText.Split(':')[1]);
            user.RevCount = int.Parse(userRateNode.SelectSingleNode("//i[2]").InnerText.Split(':')[1]);

            review.Reviever = user;
            var revDataNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"main_head_user_detail\"]");
           
            review.Date = revDataNode.SelectSingleNode("i[1]").InnerText;
           
            review.Likes = int.Parse(revDataNode.SelectSingleNode("b").InnerText);
        }

       
        void GetRevText(HtmlDocument doc, ref Review review)
        {
            review.Text = doc.DocumentNode.SelectSingleNode("//*[@itemprop=\"description\"]").InnerText;
        }

        //Parce rating div
        void GetRevRating(HtmlDocument doc, ref Review review)
        {
            var rate = new Rating();
            //Fix format string
            try
            {
                rate.Rate = int.Parse(doc.DocumentNode.SelectSingleNode("//*[@itemprop=\"ratingValue\"]").GetAttributeValue("content", "0"));
            }catch{ rate.Rate = -1; }
            rate.Usefull = IsUsefull(doc);
            rate.Ratings = ParceTargetRate(doc);
            review.Rate = rate;
        }

        //gets target ratings
        List<TargetRating> ParceTargetRate(HtmlDocument doc)
        {
            List<TargetRating> targRates = new List<TargetRating>();
            var trnode = doc.DocumentNode.SelectNodes("//*[@class=\"ext_rating_user\"]/div");
            for(int i = 0; i < trnode.Count; i = i + 3)
            {
                TargetRating tr = new TargetRating()
                {
                    Target = trnode[i].InnerText,
                    Rate=CalculateRating(trnode[i+1].SelectSingleNode("div").GetAttributeValue("style","0"))
                };
                targRates.Add(tr);
            }
            return targRates;
     
        }

        float CalculateRating(string ratestr)
        {
            ratestr = ratestr.Replace("width: ", "");
            ratestr = ratestr.Replace("px", "");
            ratestr= ratestr.Replace('.', ',');
            try
            {
                float value = (float.Parse(ratestr) / barlength) * 100;
                return value;
            }
            catch { return 0; }
        }


        bool IsUsefull(HtmlDocument doc)
        {
            string usefull = doc.DocumentNode.SelectSingleNode("//*[@class=\"recommend_user\"]/b").InnerText;
            if (usefull == "ДА") { return true; }
            else { return false; }
        }


        bool HasData(HtmlDocument doc)
        {
            string text = doc.DocumentNode.SelectSingleNode("//*[@id=\"main_content\"]/div/div/div/div/div[4]").InnerText;
            if(text.Contains("К сожалению, доступных отзывов пока не найдено."))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Parce(string revURL)
        {
            try
            {
                var review = new Review
                {
                    URL = revURL
                };

                var doc = Program.proxyConnector.Connect(revURL);
                if (HasData(doc))
                {
                   
                    review.SitePath = BuildPathString(doc);
                    BuildRevData(doc, ref review);
                    review.Name = doc.DocumentNode.SelectSingleNode("//*[@id=\"main_content\"]/div/div/div/div/div[4]/h1").InnerText;
                    review.Pluses = doc.DocumentNode.SelectSingleNode("//*[@id=\"main_content\"]/div/div/div/div/div[4]/div[2]").InnerText;
                    review.Minuses = doc.DocumentNode.SelectSingleNode("//*[@id=\"main_content\"]/div/div/div/div/div[4]/div[3]").InnerText;
                    GetRevText(doc, ref review);
                    GetRevRating(doc, ref review);
                    Program.jsonWriter.Write(review);
                    Console.WriteLine("Parced");
                }
                else { Console.WriteLine("No Data"); }
            }catch(Exception e) { Console.WriteLine("Invalid Page Format: " + e.Message); }

        }

    }
}
