using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class CollectionsClass
    {
        public static void Main()
        {
            int[] numbers = { 9,7,6,2,4,21,39,42};
            List<string> names = new List<string>(){ "simran","pavan","Samyuktha","Sravan","Tulasi"};

            // IEnumerable<Customer> Customers = Customer.GetCustomers().Where(customer => customer.Gender == "Female" && customer.Salary>40);
            // var Customers1 = (from customer in Customer.GetCustomers() orderby customer.Name.Length ascending  select customer).Reverse();

            /* var Customers = Customer.GetCustomers().OrderByDescending(x => x.Name.Length).ThenBy(x => x.Salary).Reverse();
             foreach (var c in Customers)
             Console.WriteLine(c.Name + "\t" + c.ID +"\t"+c.Salary);*/
            names.Add("Sur");
            var x = names.Where(t => t.Length > 1).ToList();
            var p = names.Min();
            //names.Add("Sur");
 foreach (var x1 in x)
     Console.WriteLine(x1);

            Console.WriteLine(p);
 Console.Read();

}
}

public class Customer
{
public int ID;
public string Name;
public string Gender;
public int Salary;

public static List<Customer> GetCustomers()
{
 return new List<Customer>
 {
     new Customer{ID=101,Name="Simran",Gender="Female",Salary=40},
     new Customer{ID=102,Name="Pavan",Gender="Male",Salary=50},
     new Customer{ID=103,Name="Samyuktha",Gender="Female",Salary=45},
     new Customer{ID=104,Name="Sravan",Gender="Male",Salary=30},
     new Customer{ID=105,Name="Tulasi",Gender="Female",Salary=42}

 };
}
}




}
