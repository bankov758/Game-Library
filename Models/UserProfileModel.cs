using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Game_Library_2._0.Models
{
    public class UserProfileModel
    {
        public UserProfileModel(string username, string email, double moneySpent,
            double averageCompletion, List<GameModel> games)
        {
            Username = username;
            Email = email;
            MoneySpent = moneySpent;
            AverageCompletion = averageCompletion;
            Games = games;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public double MoneySpent { get; set; }

        public double AverageCompletion { get; set; }

        public List<GameModel> Games { get; set; }


    }
}