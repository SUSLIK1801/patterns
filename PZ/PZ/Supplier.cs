using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class Supplier
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private string Country { get; set; }

        public Supplier() { }

        public int GetId() { return Id; }
        public string GetName() { return Name; }
        public string GetCountry() { return Country; }

        public class SupplierBuilder
        {
            private int Id { get; set; }
            private string Name { get; set; }
            private string Country { get; set; } = null;

            public SupplierBuilder SetId(int id)
            {
                Id = id;
                return this;
            }

            public SupplierBuilder SetName(string name)
            {
                Name = name;
                return this;
            }

            public SupplierBuilder SetCountry(string country)
            {
                Country = country;
                return this;
            }

            public Supplier Build()
            {
                Supplier supplier = new Supplier();
                supplier.Id = Id;
                supplier.Name = Name;
                supplier.Country = Country;
                return supplier;
            }
        }
    }
}
