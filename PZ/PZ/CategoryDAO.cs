using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class CategoryDAO : IDAO<Category>
    {
        private MySqlConnection connection;

        public CategoryDAO()
        {
            connection = DbConnection.GetInstance().GetConnection();
        }

        public List<Category> GetAll()
        {
            connection.Open();
            string SelectAllCategories = "select * from category order by category.category_id";
            MySqlCommand command = new MySqlCommand(SelectAllCategories, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                Category.CategoryBuilder builder = new Category.CategoryBuilder()
                    .SetId(reader.GetInt32(0))
                    .SetName(reader.GetString(1));

                Category category = builder.Build();
                categories.Add(category);
            }

            reader.Close();
            connection.Close();
            return categories;
            
        }

        public List<Category> GetByName(string name)
        {
            connection.Open();
            string GetByName = "select * from category where category.name LIKE @NAME order by category.category_id";
            MySqlCommand command = new MySqlCommand(GetByName, connection);
            command.Parameters.Add("@NAME", MySqlDbType.String).Value = "%" + name + "%";
            MySqlDataReader reader = command.ExecuteReader();
            List<Category> categories = new List<Category>();

            while (reader.Read())
            {
                Category.CategoryBuilder builder = new Category.CategoryBuilder()
                    .SetId(reader.GetInt32(0))
                    .SetName(reader.GetString(1));

                Category category = builder.Build();
                categories.Add(category);
            }

            reader.Close();
            connection.Close();
            return categories;
        }

        public void Add(Category category)
        {
            connection.Open();
            string insertCategory = "INSERT INTO category (name) VALUES (@NAME)";
            MySqlCommand command = new MySqlCommand(insertCategory, connection);
            command.Parameters.AddWithValue("@NAME", category.GetName());
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Категорію успішно додано до бази даних.");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні категорії.");
            }
            connection.Close();
        }

        public void Update(int ID, Category category)
        {
            connection.Open();
            string updateCategory = "UPDATE category SET name = @Name WHERE category_id = @ID";
            MySqlCommand command = new MySqlCommand(updateCategory, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            command.Parameters.AddWithValue("@Name", category.GetName());
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Категорію успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Помилка при оновленні категорії.");
            }
            connection.Close();
        }

        public void Delete(int id)
        {
            connection.Open();
            string deleteCategory = "DELETE FROM category WHERE category_id = @ID";
            MySqlCommand command = new MySqlCommand(deleteCategory, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Категорію успішно видалено.");
            }
            else
            {
                Console.WriteLine("Помилка при видаленні категорії. Можливо, запис із зазначеним ID не існує.");
            }
            connection.Close();
        }
    }
}
