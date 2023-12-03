using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public interface IObservable
    {
        void registerObserver(IObserver observer);
        void unregisterObserver(IObserver observer);
        void notifyObserver(string message);
    }
}
