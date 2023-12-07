using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PZ
{
    public class CategoryDAO : IDAO<Category>, IObservable
    {
        private MySqlConnection connection;
        private List<IObserver> observers;

        public CategoryDAO()
        {
            connection = DbConnection.GetInstance().GetConnection();
            observers = new List<IObserver>();
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
            notifyObserver($"CategoryObserver: Виведено усі категорії товарів!");
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
            notifyObserver($"CategoryObserver: Було знайдено категорію(ї) з назвою: {name}");
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
                notifyObserver($"CategoryObserver: Було додано нову категорію з назвою: {category.GetName()}");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні категорії.");
                notifyObserver($"CategoryObserver: Помилка при додаванні нової категорії з назвою: {category.GetName()}");
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
                notifyObserver($"CategoryObserver: Категорію з ID: {ID} оновлено!"); 
            }
            else
            {
                Console.WriteLine("Помилка при оновленні категорії!");
                notifyObserver($"CategoryObserver: Помилка при оновлені категорії з ID: {ID}");
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
                notifyObserver($"CategoryObserver: Категорію з ID:{id} видалено!");
            }
            else
            {
                Console.WriteLine("Помилка при видаленні категорії. Можливо, запис із зазначеним ID не існує.");
                notifyObserver($"CategoryObserver: Помилка при видаленні категорії з ID: {id}.");
            }
            connection.Close();
        }

        public void registerObserver(IObserver observer)
        {
            observers.Add(observer);
            Console.WriteLine("Слухача CategoryObserver зареєстровано!");
        }

        public void unregisterObserver(IObserver observer)
        {
            observers.Remove(observer);
            Console.WriteLine("Слухача CategoryObserver видалено!");
        }

        public void notifyObserver(string message)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(message);
            }
        }

        public void Pop()
        {
            throw new NotImplementedException();
        }
    }
}