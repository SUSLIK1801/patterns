using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PZ
{
    public class ProductDAO : IDAO<Product>, IObservable
    {
        private MySqlConnection connection;
        private List<IObserver> observers;

        public ProductDAO()
        {
            connection = DbConnection.GetInstance().GetConnection();
            observers = new List<IObserver>();
        }

        public List<Product> GetAll()
        {
            connection.Open();
            string SelectAllProducts = "SELECT products.product_id, products.name, supplier.name, category.name, products.price, products.availablity FROM products " +
                "left join supplier on products.supplier_id = supplier.supplier_id " +
                "left join category on products.category_id = category.category_id " +
                "order by products.product_id";
            MySqlCommand command = new MySqlCommand(SelectAllProducts, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product.ProductBuilder builder = new Product.ProductBuilder()
                .SetId(reader.GetInt32(0))
                .SetName(reader.GetString(1))
                .SetSupplier(reader.IsDBNull(2) ? "Невідомий" : reader.GetString(2))
                .SetCategory(reader.IsDBNull(3) ? "Невідомий" : reader.GetString(3))
                .SetPrice(reader.GetDecimal(4))
                .SetAvailable(reader.GetInt32(5));

                Product product = builder.Build();
                products.Add(product);
            }

            reader.Close();
            connection.Close();
            notifyObserver($"ProductObserver: Виведено увесь товар!");
            return products; 
        }
        
        public List<Product> GetByName(string name)
        {
            connection.Open();
            string GetByName = "SELECT products.product_id, products.name, supplier.name, category.name, products.price, products.availablity FROM products " +
                "left join supplier ON products.supplier_id = supplier.supplier_id " +
                "left join category ON products.category_id = category.category_id " +
                "WHERE products.name LIKE @NAME";
            MySqlCommand command = new MySqlCommand(GetByName, connection);
            command.Parameters.Add("@NAME", MySqlDbType.String).Value = "%" + name + "%";
            MySqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product.ProductBuilder builder = new Product.ProductBuilder()
                .SetId(reader.GetInt32(0))
                .SetName(reader.GetString(1))
                .SetSupplier(reader.IsDBNull(2) ? "Невідомий" : reader.GetString(2))
                .SetCategory(reader.IsDBNull(3) ? "Невідомий" : reader.GetString(3))
                .SetPrice(reader.GetDecimal(4))
                .SetAvailable(reader.GetInt32(5));

                Product product = builder.Build();
                products.Add(product);
            }

            reader.Close();
            connection.Close();
            notifyObserver($"ProductObserver: Було знайдено товар(и) з назвою: {name}!");
            return products;
        }

        public void Add(Product product)
        {
            connection.Open();
            string insertProduct = "INSERT INTO confectionery_store.products (name, supplier_id, category_id, price, availablity) VALUES (@PRODNAME, (select supplier.supplier_id from supplier where supplier.name = @SUPNAME), (select category.category_id from category where category.name = @CATNAME), @PRICE, @AVAILABLE)";
            MySqlCommand command = new MySqlCommand(insertProduct, connection);
            command.Parameters.AddWithValue("@PRODNAME", product.GetName());
            command.Parameters.AddWithValue("@SUPNAME", product.GetProductSupplier());
            command.Parameters.AddWithValue("@CATNAME", product.GetProductCategory());
            command.Parameters.AddWithValue("@PRICE", product.GetPrice());
            command.Parameters.AddWithValue("@AVAILABLE", product.GetAvailable());
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                notifyObserver($"ProductObserver: Було додано новий товар з назвою: {product.GetName()}, постачальником: {product.GetProductSupplier()}, категорією: {product.GetProductCategory()}, ціною: {product.GetPrice()} та кількістю: {product.GetAvailable()}.");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні товару.");
                notifyObserver($"ProductObserver: Помилка при додаванні нового товару з назвою: {product.GetName()}, постачальником: {product.GetProductSupplier()}, категорією: {product.GetProductCategory()}, ціною: {product.GetPrice()} та кількістю: {product.GetAvailable()}.");
            }
            connection.Close();
        }

        public void Update(int ID, Product product)
        {
            connection.Open();
            string updateProduct = "UPDATE confectionery_store.products SET supplier_id = (select supplier.supplier_id from supplier where supplier.name = @SUPNAME), category_id = (select category.category_id from category where category.name = @CATNAME), price = @PRICE, availablity = @AVAILABLE WHERE (product_id = @ID)";
            MySqlCommand command = new MySqlCommand(updateProduct, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            command.Parameters.AddWithValue("@SUPNAME", product.GetProductSupplier());
            command.Parameters.AddWithValue("@CATNAME", product.GetProductCategory());
            command.Parameters.AddWithValue("@PRICE", product.GetPrice());
            command.Parameters.AddWithValue("@AVAILABLE", product.GetAvailable());
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                notifyObserver($"ProductObserver: Товар з ID: {ID} оновлено!");
            }
            else
            {
                Console.WriteLine("Помилка при оновленні даних товару.");
                notifyObserver($"ProductObserver: Помилка при оновлені товару з ID: {ID}");
            }
            connection.Close();
        }

        public void Delete(int ID)
        {
            connection.Open();
            string deleteProduct = "DELETE FROM confectionery_store.products WHERE (product_id = @ID)";
            MySqlCommand command = new MySqlCommand(deleteProduct, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                notifyObserver($"ProductObserver: Товар з ID: {ID} видалено!");
            }
            else
            {
                Console.WriteLine("Помилка при видаленні товару. Можливо, запис із зазначеним ID не існує.");
                notifyObserver($"ProductObserver: Помилка при видаленні товару з ID: {ID}.");
            }
            connection.Close();
        }

        public void registerObserver(IObserver observer)
        {
            observers.Add(observer);
            Console.WriteLine("Слухача ProductObserver зареєстровано!");
        }

        public void unregisterObserver(IObserver observer)
        {
            observers.Remove(observer);
            Console.WriteLine("Слухача ProductObserver видалено!");
        }

        public void notifyObserver(string message)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(message);
            }
        }
    }
}
