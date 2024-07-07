using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EFGetStarted
{
    internal class Program
    {
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
                    throw new Exception("Invalid Operation!");
            }
            return;
        }
        public static DbSet<T> RequestedTable<T>(int tableNum) where T : class
        {
            EFGetStarted.NorthWindEntities context = new NorthWindEntities();
            switch (tableNum)
            {
                case 0:
                    return context.Set<CustomerDemographic>() as DbSet<T>;
                case 1:
                    return context.Set<Customer>() as DbSet<T>;
                case 2:
                    return context.Set<Order>() as DbSet<T>;
                case 3:
                    return context.Set<Region>() as DbSet<T>;
                case 4:
                    return context.Set<Employee>() as DbSet<T>;
                case 5:
                    return context.Set<Territory>() as DbSet<T>;
                case 6:
                    return context.Set<Shipper>() as DbSet<T>;
                case 7:
                    return context.Set<Supplier>() as DbSet<T>;
                case 8:
                    return context.Set<Category>() as DbSet<T>;
                case 9:
                    return context.Set<Product>() as DbSet<T>;
                default:
                    throw new Exception("Invalid Number! \t Choose Numbers Between 0 and 9 Only");
            }

        }

     

        static void Main(string[] args)
        {
           
            EFGetStarted.NorthWindEntities context = new NorthWindEntities();

            int tableNum = 5;
            int opertaionNum = 3;

            var content = RequestedTable<NorthWindEntities>(tableNum);




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



            //deleteCustomer(8);
            //DisplayCustomers("Deleting ID 8");
            //#endregion
            //deleteCustomer(2);
            //DisplayCustomers("Deleting ID 2");


             bool exit = false;
            bool inSubMenu = false;
            DbSet selectedTable = null;

            while (!exit)
            {
                if (!inSubMenu)
                {
                    Console.WriteLine("Select a table to navigate:");
                    Console.WriteLine("0: CustomerDemographic");
                    Console.WriteLine("1: Customer");
                    Console.WriteLine("2: Order");
                    Console.WriteLine("3: Region");
                    Console.WriteLine("4: Employee");
                    Console.WriteLine("5: Territory");
                    Console.WriteLine("6: Shipper");
                    Console.WriteLine("7: Supplier");
                    Console.WriteLine("8: Category");
                    Console.WriteLine("9: Product");
                    Console.WriteLine("Press '[' to go back to the main menu.");
                    Console.WriteLine("Press 'Esc' to quit.");
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // intercept: true to prevent the key from being shown in the console

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    exit = true;
                    continue;
                }

                if (inSubMenu && keyInfo.Key == ConsoleKey.Oem4) // '[' key
                {
                    inSubMenu = false;
                    selectedTable = null;
                    Console.Clear();
                    continue;
                }

                if (!inSubMenu)
                {
                    context = new NorthWindEntities(); // assuming NorthWindEntities is the context class

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D0:
                        case ConsoleKey.NumPad0:
                            selectedTable = RequestedTable<CustomerDemographic>(0);
                            Console.WriteLine("CustomerDemographic table selected.");
                            break;
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            selectedTable = RequestedTable<Customer>(1);
                            Console.WriteLine("Customer table selected.");
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            selectedTable = RequestedTable<Order>(2);
                            Console.WriteLine("Order table selected.");
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            selectedTable = RequestedTable<Region>(3);
                            Console.WriteLine("Region table selected.");
                            break;
                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4:
                            selectedTable = RequestedTable<Employee>(4);
                            Console.WriteLine("Employee table selected.");
                            break;
                        case ConsoleKey.D5:
                        case ConsoleKey.NumPad5:
                            selectedTable = RequestedTable<Territory>(5);
                            Console.WriteLine("Territory table selected.");
                            break;
                        case ConsoleKey.D6:
                        case ConsoleKey.NumPad6:
                            selectedTable = RequestedTable<Shipper>(6);
                            Console.WriteLine("Shipper table selected.");
                            break;
                        case ConsoleKey.D7:
                        case ConsoleKey.NumPad7:
                            selectedTable = RequestedTable<Supplier>(7);
                            Console.WriteLine("Supplier table selected.");
                            break;
                        case ConsoleKey.D8:
                        case ConsoleKey.NumPad8:
                            selectedTable = RequestedTable<Category>(8);
                            Console.WriteLine("Category table selected.");
                            break;
                        case ConsoleKey.D9:
                        case ConsoleKey.NumPad9:
                            selectedTable = RequestedTable<Product>(9);
                            Console.WriteLine("Product table selected.");
                            break;
                        default:
                            Console.WriteLine("Invalid key. Please try again.");
                            continue;
                    }

                    inSubMenu = true;
                }
            }
        }

        

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


        
    }
}
