using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Parcing.Models
{
    public class Review
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string URL { get; set; }
        public string SitePath { get; set; }
        public string Date { get; set; }
        public User Reviever { get; set; }
        public int Likes { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Pluses { get; set; }
        public string Minuses { get; set; }
        //public Area RevArea { get; set; }
        public Rating Rate { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int RateId { get; set; }
    }

    public class User
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public int Reputation { get; set; }
        public int RevCount { get; set; }
        [JsonIgnore]
        public Review Rev { get; set; }
    }


    public class Rating
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public int Rate { get; set; }
        public bool Usefull { get; set; }
        [JsonIgnore]
        public Review Rev { get; set; }
        public List<TargetRating> Ratings { get; set; }
    }

    public class TargetRating
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Target { get; set; }
        public float Rate { get; set; }

        //Navigation Property
        [JsonIgnore]
        public Rating NavRate { get; set; }
        [JsonIgnore]
        public int ReviewId { get; set; }

    }
}
