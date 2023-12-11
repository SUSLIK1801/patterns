using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class ProxyUserDAO : IDAO<Product>, IObservable
    {
        private UserRole role;
        private ProductDAO productdao;

        public ProxyUserDAO(UserRole role)
        {
            this.role = role;
            this.productdao = new ProductDAO();
        }

        public List<Product> GetAll()
        {
            return productdao.GetAll();
        }

        public List<Product> GetByName(string name)
        {
            return productdao.GetByName(name);
        }

        public void Add(Product product)
        {
            if(role == UserRole.admin)
            {
                productdao.Add(product);
            }
            else
            {
                Console.WriteLine("Відмовлено у доступі!");
            }
        }

        public void Update(int ID, Product product)
        {
            if (role == UserRole.admin)
            {
                productdao.Update(ID, product);
            }
            else
            {
                Console.WriteLine("Відмовлено у доступі!");
            }
        }

        public void Delete(int ID)
        {
            if (role == UserRole.admin)
            {
                productdao.Delete(ID);
            }
            else
            {
                Console.WriteLine("Відмовлено у доступі!");
            }
        }

        public void Pop()
        {
            ((IDAO<Product>)productdao).Pop();
        }

        public void registerObserver(IObserver observer)
        {
            ((IObservable)productdao).registerObserver(observer);
        }

        public void unregisterObserver(IObserver observer)
        {
            ((IObservable)productdao).unregisterObserver(observer);
        }

        public void notifyObserver(string message)
        {
            ((IObservable)productdao).notifyObserver(message);
        }
    }
}
