using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public interface IDAO<T>
    {
        List<T> GetAll();
        List<T> GetByName(string name);
        void Add(T obj);
        void Update(int ID, T obj);
        void Delete(int ID);
    }
}
