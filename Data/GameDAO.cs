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

        public List<GameModel> FetchAll()
        {
            List<GameModel> gameModels = new List<GameModel>();

            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select * from Games ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GameModel model = new GameModel();
                        model.Id = reader.GetInt32(0);
                        model.Name = reader.GetString(1);
                        model.Description = reader.GetString(2);
                        model.Completion = (float)reader.GetDecimal(3);
                        model.Price = (float)reader.GetDecimal(4);
                        model.Publisher = reader.GetString(5);
                        model.PicturePath = reader.GetString(6);

                        gameModels.Add(model);
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
                        model.Id = reader.GetInt32(0);
                        model.Name = reader.GetString(1);
                        model.Description = reader.GetString(2);
                        model.Completion = (float)reader.GetDecimal(3);
                        model.Price = (float)reader.GetDecimal(4);
                        model.Publisher = reader.GetString(5);
                        model.PicturePath = reader.GetString(6);
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
                sqlQuery = " insert into Games Values(@Name, @Description, @Completion, @Price, @Publisher, @PicturePath) ";
            }
            else
            {
                sqlQuery = " update Games set Name = @Name, Description = @Description, Completion = @Completion, " +
                    " Price = @Price, Publisher = @Publisher, PicturePath = @PicturePath where Id = @Id ";
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

        public List<GameModel> SearchForName(string searchPhrase)
        {
            List<GameModel> gameModels = new List<GameModel>();

            using (SqlConnection connection = new SqlConnection(conntectionString))
            {
                string sqlQuery = " select * from Games where name like @searchPhrase ";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add("@searchPhrase", System.Data.SqlDbType.NVarChar).Value = "%" + searchPhrase + "%";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GameModel model = new GameModel();
                        model.Id = reader.GetInt32(0);
                        model.Name = reader.GetString(1);
                        model.Description = reader.GetString(2);
                        model.Completion = (float)reader.GetDecimal(3);
                        model.Price = (float)reader.GetDecimal(4);
                        model.Publisher = reader.GetString(5);

                        gameModels.Add(model);
                    }
                }
            }
            return gameModels;
        }

    }
}