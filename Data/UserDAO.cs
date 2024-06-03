using Game_Library_2._0.Models;
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
                string sqlQuery = " select * from Users where Username = @username ";
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
                        model.Password = reader.GetString(2);
                        return model;
                    }
                }
                return model;
            }
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

        public List<UserModel> SearchForName(string searchPhrase)
        {
            List<UserModel> userModels = new List<UserModel>();

            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select * from Users where Username = @searchPhrase ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@searchPhrase", System.Data.SqlDbType.NVarChar).Value = searchPhrase;
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
    }
}