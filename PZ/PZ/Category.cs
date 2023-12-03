using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class Category
    {
        private int Id { get; set; }
        private string Name { get; set; }

        public Category() { }

        public int GetId() { return Id; }
        public string GetName() { return Name; }

        public class CategoryBuilder
        {
            private int Id { get; set; }
            private string Name { get; set; }

            public CategoryBuilder SetId(int id)
            {
                Id = id;
                return this;
            }

            public CategoryBuilder SetName(string name)
            {
                Name = name;
                return this;
            }

            public Category Build()
            {
                Category category = new Category();
                category.Id = Id;
                category.Name = Name;
                return category;
            }
        }
    }
}
