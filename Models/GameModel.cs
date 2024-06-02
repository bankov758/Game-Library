using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Game_Library_2._0.Models
{
    public class GameModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Completion { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string PicturePath { get; set; }
        public HttpPostedFileBase Picture { get; set; }

        public GameModel()
        {
            Id = -1;
            Name = "Game Name";
            Description = "Description";
            Completion = 0.0f;
            Price = 0.00f;
            Publisher = "Publisher";
            PicturePath = "";
        }

        public GameModel(int id, string name, string description, float completion, float price,
            string publisher, HttpPostedFileBase picture, string picturePath)
        {
            Id = id;
            Name = name;
            Description = description;
            Completion = completion;
            Price = price;
            Publisher = publisher;
            Picture = picture;
            PicturePath = picturePath;
        }

    }

}