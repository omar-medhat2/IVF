using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Northwind_EF
{
    internal class Program
    {
        #region List
        /*public static List<Cusomer> Cutomers = new List<Cusomer>()
            {

                new Cusomer(){ Id= 1 , Jop = "softwer engineer" , Name ="ali hassan" , Salary=20000},
                new Cusomer(){ Id= 2 , Jop = "QC engineer" , Name ="Hossam hassan" , Salary=50000},
                new Cusomer(){ Id= 3 , Jop = "civil engineer" , Name ="omar ali" , Salary=40000},
            };
        */
        #endregion

        public static DbSet<T> RequestedTable<T>(int tableNum) where T : class
        {
            Northwind_EF.NorthwindEntities context = new NorthwindEntities();
            switch (tableNum)
            {
                case 0:
                    return context.Set<Order>() as DbSet<T>;
                case 1:
                    return context.Set<Customer>() as DbSet<T>;
                case 2:
                    return context.Set<Region>() as DbSet<T>;
                case 3:
                    return context.Set<Employee>() as DbSet<T>;
                case 4:
                    return context.Set<Territory>() as DbSet<T>;
                case 5:
                    return context.Set<Shipper>() as DbSet<T>;
                case 6:
                    return context.Set<Category>() as DbSet<T>;
                case 7:
                    return context.Set<Product>() as DbSet<T>;
                case 8:
                    return context.Set<Supplier>() as DbSet<T>;
                case 9:
                    return context.Set<CustomerDemographic>() as DbSet<T>;
                default:
                    throw new Exception("Invalid Number! \t Choose Numbers Between 0 and 9 Only");
            }

        }

        public static String DecideOperation(int operation)
        {
            switch (operation)
            {
                case 1:
                    return "create";
                case 2:
                    return "update";
                case 3:
                    return "delete";
                case 4:
                    return "traverse";
                case 5:
                    return "read";
                default:
                    throw new Exception("Invalid Number! \t Choose Number Between 1 and 5 Only");
            }

        }

        static void Main(string[] args)
        {
            #region commented
            Northwind_EF.NorthwindEntities context = new NorthwindEntities();

            int tableNum = 5;
            int opertaionNum = 3;

            var content = RequestedTable<NorthwindEntities>(tableNum);
            string operation = DecideOperation(opertaionNum);



            using (context = new NorthwindEntities())
            {
                var newCustomer = context.Customers.FirstOrDefault(c => c.CustomerID == "ADHAM");

                if (newCustomer != null)
                {
                    context.Customers.Remove(newCustomer);
                    context.SaveChanges();

                    Console.WriteLine("Customer removed successfully.");
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }

                using (context = new NorthwindEntities())
                {
                    var customers = context.Customers.Distinct();

                    foreach (var customer in customers)
                    {
                        Console.WriteLine("CompanyName: {0}, ContactName: {1}, City: {2}, Phone: {3}, Address: {4}, Country: {5}",
                            customer.CompanyName,
                            customer.ContactName,
                            customer.City,
                            customer.Phone,
                            customer.Address,
                            customer.Country);
                    }
                }
            }
            #endregion


            #region to comment
            //DisplayCustomers("Before cutomers list");

            //var newcustomer = new Cusomer();
            //newcustomer.Name = "mohamed hassan";
            //newcustomer.Id = 4;
            //newcustomer.Salary = 10000;
            //newcustomer.Jop = "DB Administrator";
            //#region insert
            //insertCustomer(newcustomer);
            //DisplayCustomers("After add cutomers list");
            //#endregion

            //#region edit for customer id 1  (username  = hisham)
            //editName(1,"Adham");
            //DisplayCustomers("After edit cutomers list");
            #endregion

            #region delete
            //deleteCustomer(8);
            //DisplayCustomers("Deleting ID 8");
            //#endregion
            //deleteCustomer(2);
            //DisplayCustomers("Deleting ID 2");

            #endregion

        }

        #region Methods

        ///// <summary>
        ///// Display All Current Customers in console
        ///// </summary>
        ///// <param name="title"></param>
        //public static void DisplayCustomers(string title)
        //{
        //    System.Console.WriteLine(title);
        //    foreach (var customer in Cutomers)
        //    {
        //        System.Console.WriteLine($"ID= {customer.Id} \t Name = {customer.Name}");

        //    }
        //}


        ///// <summary>
        ///// Get Customer by ID
        ///// </summary>
        ///// <param name="inputID"></param>
        ///// <returns></returns>
        //public Cusomer getCustomer(int inputID)
        //{
        //    foreach (var customer in Cutomers)
        //    {
        //        if (customer.Id == inputID)
        //        {
        //            return customer;
        //        }
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Checks if customer exists, if not then customer is added
        ///// </summary>
        ///// <param name="ctmr"></param>
        //public static void insertCustomer(Cusomer ctmr)
        //{
        //    foreach(var customer in Cutomers)
        //    {
        //        if (customer.Id==ctmr.Id)
        //        {
        //            Console.WriteLine("Customer Already Exists");
        //            break;
        //        }

        //    }
        //    Cutomers.Add(ctmr);
        //    Console.WriteLine("Customer Added");
        //    return;
        //}


        ///// <summary>
        ///// checks for ID and changes the name if found
        ///// </summary>
        ///// <param name="customerID"></param>
        ///// <param name="nameToChange"></param>
        //public static void editName(int customerID, string nameToChange)
        //{
        //    foreach (var customer in Cutomers)
        //    {
        //        if (customer.Id==customerID)
        //        {
        //            customer.Name = nameToChange;
        //            return;
        //        }
        //    }
        //    Console.WriteLine("ID was not found!");
        //    return;
        //}

        /// <summary>
        /// Deletes Customer if found 
        /// </summary>
        /// <param name="customerID"></param>
        public static void PerformCrudOperation<T>(T data, DbContext context, string operation) where T : class
        {
            //Northwind_EF.NorthwindEntities context = new NorthwindEntities();
            var table = context.Set<T>();


            switch (operation.ToLower())
            {
                case "add":
                    table.Add(data);
                    Console.WriteLine("${operation} is complete");
                    break;
                case "update":
                    table.AddOrUpdate(data);
                    Console.WriteLine("${operation} is complete");
                    break;
                case "delete":
                    table.Remove(data);
                    Console.WriteLine("${operation} is complete");
                    break;
                case "traverse":
                    var list = table.ToList();
                    Console.WriteLine("${operation} is complete");
                    break;
                case "read":
                    var array = table.ToList();
                    Console.WriteLine("${operation} is complete");
                    break;
                default:
                    throw new Exception("Invalid Operation!");
            }
            return;
        }

        #endregion
    }
}





