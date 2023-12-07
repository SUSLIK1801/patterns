using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class ProductMemento
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ProductSupplier { get; private set; }
        public string ProductCategory { get; private set; }
        public decimal Price { get; private set; }
        public int Available { get; private set; }

        public ProductMemento(int id, string name, string supplier, string category, decimal price, int available)
        {
            this.Id = id;
            this.Name = name;
            this.ProductSupplier = supplier;
            this.ProductCategory = category;
            this.Price = price;
            this.Available = available;
        }
    }
}
