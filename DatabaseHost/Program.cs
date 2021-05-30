using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DatabaseHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ClientServer.DatabaseService)))
            {
                host.Open();
                Console.WriteLine("Старт");
                Console.ReadLine();
            }
        }
    }
}
