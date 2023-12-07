using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class ProductCaretaker
    {
        public Stack<ProductMemento> mementos { get; private set; }

        public ProductCaretaker()
        {
            mementos = new Stack<ProductMemento>();
        }
        
        public void Push(ProductMemento memento)
        {
            mementos.Push(memento);
            //Console.WriteLine($"ProductCaretaker: Додано момент, кількість моментів: {mementos.Count}");
        }

        public ProductMemento Pop()
        {
            return mementos.Pop();
        }
    }
}
