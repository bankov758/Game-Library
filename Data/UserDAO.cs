using Game_Library_2._0.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;

namespace Game_Library_2._0.Data
{
    public class UserDAO
    {
        private string conntectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Game Library\";" +
    "Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public string LoggedUsername { get; set; }

        public UserDAO() { }

        public UserDAO(string loggedUsername) { LoggedUsername = loggedUsername; }

        public UserModel Fetch(string username)
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select Id, Username, Email, Password from Users where Username = @username ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = username;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                UserModel model = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model = new UserModel();
                        model.Id = reader.GetInt32(0);
                        model.Username = reader.GetString(1);
                        model.Email = reader.GetString(2);
                        model.Password = reader.GetString(3);
                        return model;
                    }
                }
                return model;
            }
        }

        public UserModel Fetch(int id)
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select Id, Username, Email, Password from Users where Id = @Id ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                UserModel model = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model = new UserModel();
                        model.Id = reader.GetInt32(0);
                        model.Username = reader.GetString(1);
                        model.Email = reader.GetString(2);
                        model.Password = reader.GetString(3);
                        return model;
                    }
                }
                return model;
            }
        }

        public List<UserProfileModel> FetchFriends()
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select Id, Username, Email, Password from Users where Id in (select FriendId from Friends where UserId = @Id) ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = Fetch(LoggedUsername).Id; ;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                List<UserProfileModel> friends = new List<UserProfileModel>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserModel model = new UserModel();
                        model.Id = reader.GetInt32(0);
                        model.Username = reader.GetString(1);
                        model.Email = reader.GetString(2);
                        model.Password = reader.GetString(3);
                        friends.Add(geUserProfileFromtUserModel(model));
                    }
                }
                return friends;
            }
        }

        public UserProfileModel FetchUserProfile(string username)
        {
            UserModel userModel = Fetch(username);
            return geUserProfileFromtUserModel(userModel);
        }

        public UserProfileModel FetchUserProfile(int id)
        {
            UserModel userModel = Fetch(id);
            return geUserProfileFromtUserModel(userModel);
        }

        private UserProfileModel geUserProfileFromtUserModel(UserModel userModel) {
            GameDAO gameDAO = new GameDAO();
            gameDAO.LoggedUsername = userModel.Username;
            List<GameModel> userGames = gameDAO.FetchAll();
            double moneySpent = 0;
            double avgCompletion = 0;
            foreach (GameModel gameModel in userGames)
            {
                moneySpent += gameModel.Price;
                avgCompletion += gameModel.Completion;
            }
            avgCompletion = Math.Round(avgCompletion / userGames.Count, 2);
            return new UserProfileModel(userModel.Id, userModel.Username, userModel.Email, Math.Round(moneySpent, 2), avgCompletion, userGames);
        }

        public int CreateOrUpdate(UserModel user)
        {
            string sqlQuery = "";
            if (user.Id <= 0)
            {
                sqlQuery = " insert into Users Values(@Username, @Password, @Email) ";
            }
            else
            {
                sqlQuery = " update Users set Username = @Username, Password = @Password, Email = @Email where Id = @Id ";
            }
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                if (user.Id > 0)
                {
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = user.Id;
                }
                command.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar, 100).Value = user.Username;
                command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 100).Value = Crypto.HashPassword(user.Password); ;
                command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 100).Value = user.Email;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public List<UserModel> SearchUsername(string searchUsername)
        {
            List<UserModel> userModels = new List<UserModel>();

            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select Id, Username, Email from Users where Username like @searchUsername " +
                    " and Username != @loggedUsername and Id not in (select FriendId from Friends where UserId = @Id)";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@searchUsername", System.Data.SqlDbType.NVarChar).Value = "%" + searchUsername + "%";
                command.Parameters.Add("@loggedUsername", System.Data.SqlDbType.NVarChar).Value = LoggedUsername;
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = Fetch(LoggedUsername).Id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserModel model = new UserModel();
                        model.Id = reader.GetInt32(0);
                        model.Username = reader.GetString(1);
                        model.Email = reader.GetString(2);
                        userModels.Add(model);
                    }
                }
            }
            return userModels;
        }

        public void addFriend(int friendId) { 
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " insert into Friends Values(@UserId, @FriendId) ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Fetch(LoggedUsername).Id;
                command.Parameters.Add("@FriendId", System.Data.SqlDbType.Int).Value = friendId;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveFriend(int friendId)
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " delete from Friends where UserId = @UserId and FriendId = @FriendId ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Fetch(LoggedUsername).Id;
                command.Parameters.Add("@FriendId", System.Data.SqlDbType.Int).Value = friendId;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}