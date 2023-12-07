using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class Product
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string ProductSupplier { get; set; }
        private string ProductCategory { get; set; }
        private decimal Price { get; set; }
        private int Available { get; set; }


        public Product(){}

        public int GetId() { return Id; }
        public string GetName() { return Name; }
        public string GetProductSupplier() { return ProductSupplier; }
        public string GetProductCategory() {  return ProductCategory; }
        public decimal GetPrice() { return Price; }
        public int GetAvailable() { return Available; }


        public class ProductBuilder
        {
            private int Id { get; set; }
            private string Name { get; set; }
            private string ProductSupplier { get; set; } = null;
            private string ProductCategory { get; set; } = null;
            private decimal Price { get; set; } = 0.0m;
            private int Available { get; set; } = 0;

            public ProductBuilder SetId(int id)
            {
                Id = id;
                return this;
            }

            public ProductBuilder SetName(string name)
            {
                Name = name;
                return this;
            }

            public ProductBuilder SetSupplier(string supplier)
            {
                ProductSupplier = supplier;
                return this;
            }

            public ProductBuilder SetCategory(string category)
            {
                ProductCategory =category;
                return this;
            }

            public ProductBuilder SetPrice(decimal price)
            {
                Price = price;
                return this;
            }

            public ProductBuilder SetAvailable(int available)
            {
                Available = available;
                return this;
            }

            public Product Build()
            {
                Product product = new Product();
                product.Id = Id;
                product.Name = Name;
                product.ProductSupplier = ProductSupplier;
                product.ProductCategory = ProductCategory;
                product.Price = Price;
                product.Available = Available;
                return product;
            }
        }

        public ProductMemento SaveState()
        {
            Console.WriteLine($"Зберігання товару (ID: {Id}, Назва: {Name}, Постачальник: {ProductSupplier}, Категорія: {ProductCategory}, Ціна: {Price}, Кількість: {Available}.)");
            return new ProductMemento(Id, Name, ProductSupplier, ProductCategory, Price, Available);
        }

        public void RestoreState(ProductMemento memento)
        {
            Id = memento.Id;
            Name = memento.Name;
            ProductSupplier = memento.ProductSupplier;
            ProductCategory = memento.ProductCategory;
            Price = memento.Price;
            Available = memento.Available;
        }
    }
}
