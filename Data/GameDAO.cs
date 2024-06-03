using Game_Library_2._0.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace Game_Library_2._0.Data
{
    public class GameDAO
    {

        private string conntectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"Game Library\";" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public string LoggedUsername { get; set; }

        public List<GameModel> FetchAll()
        {
            List<GameModel> gameModels = new List<GameModel>();
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select * from Games where UserId = @UserId";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = getLoggedUserId(LoggedUsername);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        gameModels.Add(getGameModelFromSqlResult(reader));
                    }
                }
            }
            return gameModels;
        }

        public GameModel Fetch(int Id)
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select * from Games where Id = @id";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = Id;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GameModel model = new GameModel();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return getGameModelFromSqlResult(reader);
                    }
                }
                return model;
            }
        }

        public int CreateOrUpdate(GameModel gameModel)
        {
            string sqlQuery = "";
            if (gameModel.Id <= 0)
            {
                sqlQuery = " insert into Games Values(@Name, @Description, @Completion, @Price, @Publisher, @PicturePath, @UserId) ";
            }
            else
            {
                sqlQuery = " update Games set Name = @Name, Description = @Description, Completion = @Completion, " +
                    " Price = @Price, Publisher = @Publisher, PicturePath = @PicturePath, UserId = @UserId where Id = @Id ";
            }
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                if (gameModel.Id > 0)
                {
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = gameModel.Id;
                }
                command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 100).Value = gameModel.Name;
                command.Parameters.Add("@Description", System.Data.SqlDbType.NText).Value = gameModel.Description;
                command.Parameters.Add("@Completion", System.Data.SqlDbType.Decimal).Value = gameModel.Completion;
                command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal).Value = gameModel.Price;
                command.Parameters.Add("@Publisher", System.Data.SqlDbType.VarChar, 100).Value = gameModel.Publisher;
                command.Parameters.Add("@PicturePath", System.Data.SqlDbType.VarChar, 40000).Value = gameModel.PicturePath;
                command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = getLoggedUserId(LoggedUsername);
                Console.WriteLine(command.CommandText);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " delete from Games where Id = @Id ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 100).Value = id;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public List<GameModel> Search(GameSearchModel searchModel)
        {
            List<GameModel> gameModels = new List<GameModel>();
            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = BuildSearchQuery(searchModel);
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = getLoggedUserId(LoggedUsername);
                if (searchModel.Name != null)
                {
                    command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = "%" + searchModel.Name + "%";
                }
                if (searchModel.Publisher != null)
                {
                    command.Parameters.Add("@Publisher", System.Data.SqlDbType.NVarChar).Value = "%" + searchModel.Publisher + "%";
                }
                if (searchModel.Price != 0)
                {
                    command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal).Value = searchModel.Price;
                }
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        gameModels.Add(getGameModelFromSqlResult(reader));
                    }
                }
            }
            return gameModels;
        }

        private string BuildSearchQuery(GameSearchModel searchModel)
        {
            string sqlQuery = " select * from Games where UserId = @UserId ";
            if (searchModel.Name != null)
            {
                sqlQuery += " and Name like @Name ";
            }
            if (searchModel.Publisher != null)
            {
                sqlQuery += " and Publisher like @Publisher ";
            }
            if (searchModel.Price != 0)
            {
                sqlQuery += " and Price " + searchModel.Operator + " @Price ";
            }
            return sqlQuery;
        }

        private GameModel getGameModelFromSqlResult(SqlDataReader reader)
        {
            GameModel model = new GameModel();
            model.Id = reader.GetInt32(0);
            model.Name = reader.GetString(1);
            model.Description = reader.GetString(2);
            model.Completion = (float)reader.GetDecimal(3);
            model.Price = (float)reader.GetDecimal(4);
            model.Publisher = reader.GetString(5);
            model.PicturePath = reader.GetString(6);
            return model;
        }

        private int getLoggedUserId(string loggedUsername)
        {
            UserDAO userDAO = new UserDAO();
            return userDAO.Fetch(loggedUsername).Id;
        }

    }
}