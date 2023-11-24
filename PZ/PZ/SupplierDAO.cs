using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class SupplierDAO : IDAO<Supplier>
    {
        private MySqlConnection connection;

        public SupplierDAO()
        {
            connection = DbConnection.GetInstance().GetConnection();
        }

        public List<Supplier> GetAll()
        {
            connection.Open();
            string SelectAllSuppliers = "select * from supplier order by supplier.supplier_id";
            MySqlCommand command = new MySqlCommand(SelectAllSuppliers, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<Supplier> suppliers = new List<Supplier>();
            while (reader.Read())
            {
                Supplier.SupplierBuilder builder = new Supplier.SupplierBuilder()
                    .SetId(reader.GetInt32(0))
                    .SetName(reader.GetString(1))
                    .SetCountry(reader.IsDBNull(2) ? "Невідома" : reader.GetString(2));

                Supplier supplier = builder.Build();
                suppliers.Add(supplier);
            }

            reader.Close();
            connection.Close();
            return suppliers;
        }

        public List<Supplier> GetByName(string name)
        {
            connection.Open();
            string GetByName = "select * from supplier where supplier.name LIKE @NAME order by supplier.supplier_id";
            MySqlCommand command = new MySqlCommand(GetByName, connection);
            command.Parameters.Add("@NAME", MySqlDbType.String).Value = "%" + name + "%";
            MySqlDataReader reader = command.ExecuteReader();
            List<Supplier> suppliers = new List<Supplier>();
            while (reader.Read())
            {
                Supplier.SupplierBuilder builder = new Supplier.SupplierBuilder()
                    .SetId(reader.GetInt32(0))
                    .SetName(reader.GetString(1))
                    .SetCountry(reader.IsDBNull(2) ? "Невідома" : reader.GetString(2));

                Supplier supplier = builder.Build();
                suppliers.Add(supplier);
            }

            reader.Close();
            connection.Close();
            return suppliers;
        }

        public void Add(Supplier supplier)
        {
            connection.Open();
            string insertSupplier = "INSERT INTO supplier (name, country) VALUES (@NAME, @COUNTRY)";
            MySqlCommand command = new MySqlCommand(insertSupplier, connection);
            command.Parameters.AddWithValue("@NAME", supplier.GetName());
            command.Parameters.AddWithValue("@COUNTRY", supplier.GetCountry());
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Постачальника успішно додано до бази даних.");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні постачальника.");
            }
            connection.Close();
        }

        public void Update(int ID, Supplier supplier)
        {
            connection.Open();
            string updateSupplier = "UPDATE confectionery_store.supplier SET name = @NAME, country = @COUNTRY WHERE (supplier_id = @ID)";
            MySqlCommand command = new MySqlCommand(updateSupplier, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            command.Parameters.AddWithValue("@NAME", supplier.GetName());
            command.Parameters.AddWithValue("@COUNTRY", supplier.GetCountry());
            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected > 0)
            {
                Console.WriteLine("Дані постачальника успішно оновлено.");
            }
            else
            {
                Console.WriteLine("Помилка при оновленні даних постачальника.");
            }
            connection.Close();
        }

        public void Delete(int ID)
        {
            connection.Open();
            string deleteSupplier = "DELETE FROM supplier WHERE supplier_id = @ID";
            MySqlCommand command = new MySqlCommand(deleteSupplier, connection);
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID;
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Постачальника успішно видалено.");
            }
            else
            {
                Console.WriteLine("Помилка при видаленні постачальника. Можливо, запис із зазначеним ID не існує.");
            }
            connection.Close();
        }
    }
}
