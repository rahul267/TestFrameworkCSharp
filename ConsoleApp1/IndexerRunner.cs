using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary2.Indexers;

namespace ConsoleApp1
{
    class SampleCollection<T>
    {
        // Declare an array to store the data elements.
        private T[] arr = new T[100];

        // Define the indexer to allow client code to use [] notation.
        public T this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }
    }

    class Program
    {
        //static void Main()
        //{
        //    var stringCollection = new SampleCollection<string>();
        //    stringCollection[0] = "Hello, World";
        //    Console.WriteLine(stringCollection[0]);
        //    Console.WriteLine("Press any key to exit.");
        //    Console.ReadKey();
        //}
    }
}
