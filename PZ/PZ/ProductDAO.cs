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
    public class ProductDAO : IDAO<Product>
    {
        private MySqlConnection connection;

        public ProductDAO()
        {
            connection = DbConnection.GetInstance().GetConnection();
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
                Console.WriteLine("Товар успішно додано до бази даних.");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні товару.");
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
                Console.WriteLine("Дані товару успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Помилка при оновленні даних товару.");
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
                Console.WriteLine("Товар успішно видалено.");
            }
            else
            {
                Console.WriteLine("Помилка при видаленні товару. Можливо, запис із зазначеним ID не існує.");
            }
            connection.Close();
        }
    }
}
