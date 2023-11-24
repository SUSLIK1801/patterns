using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class DAOFactory
    {
        public IDAO<T> CreateDAO<T>() where T : new()
        {
            if (typeof(T) == typeof(Product))
            {
                return new ProductDAO() as IDAO<T>;
            }
            if (typeof(T) == typeof(Category))
            {
                return new CategoryDAO() as IDAO<T>;
            }
            if (typeof(T) == typeof(Supplier))
            {
                return new SupplierDAO() as IDAO<T>;
            }
            throw new NotSupportedException("Даний тип DAO не підтримується.");
        }
    }
}
