using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;


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
            Northwind_EF.NorthwindEntities context = new NorthwindEntities();
            using (context = new NorthwindEntities())
            {
                Console.WriteLine("Welcome, Admin!\n");
                bool exit = false;
                bool inSubMenu = false;
                DbSet selectedTable = null;
                Type selectedTableType = null;

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
                        context = new NorthwindEntities(); // assuming NorthWindEntities is the context class

                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.D0:
                            case ConsoleKey.NumPad0:
                                selectedTable = RequestedTable<CustomerDemographic>(0);
                                selectedTableType = typeof(CustomerDemographic);
                                Console.WriteLine("CustomerDemographic table selected.");
                                break;
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:
                                selectedTable = RequestedTable<Customer>(1);
                                selectedTableType = typeof(Customer);
                                Console.WriteLine("Customer table selected.");
                                break;
                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2:
                                selectedTable = RequestedTable<Order>(2);
                                selectedTableType = typeof(Order);
                                Console.WriteLine("Order table selected.");
                                break;
                            case ConsoleKey.D3:
                            case ConsoleKey.NumPad3:
                                selectedTable = RequestedTable<Region>(3);
                                selectedTableType = typeof(Region);
                                Console.WriteLine("Region table selected.");
                                break;
                            case ConsoleKey.D4:
                            case ConsoleKey.NumPad4:
                                selectedTable = RequestedTable<Employee>(4);
                                selectedTableType = typeof(Employee);
                                Console.WriteLine("Employee table selected.");
                                break;
                            case ConsoleKey.D5:
                            case ConsoleKey.NumPad5:
                                selectedTable = RequestedTable<Territory>(5);
                                selectedTableType = typeof(Territory);
                                Console.WriteLine("Territory table selected.");
                                break;
                            case ConsoleKey.D6:
                            case ConsoleKey.NumPad6:
                                selectedTable = RequestedTable<Shipper>(6);
                                selectedTableType = typeof(Shipper);
                                Console.WriteLine("Shipper table selected.");
                                break;
                            case ConsoleKey.D7:
                            case ConsoleKey.NumPad7:
                                selectedTable = RequestedTable<Supplier>(7);
                                selectedTableType = typeof(Supplier);
                                Console.WriteLine("Supplier table selected.");
                                break;
                            case ConsoleKey.D8:
                            case ConsoleKey.NumPad8:
                                selectedTable = RequestedTable<Category>(8);
                                selectedTableType = typeof(Category);
                                Console.WriteLine("Category table selected.");
                                break;
                            case ConsoleKey.D9:
                            case ConsoleKey.NumPad9:
                                selectedTable = RequestedTable<Product>(9);
                                selectedTableType = typeof(Product);
                                Console.WriteLine("Product table selected.");
                                break;
                            default:
                                Console.WriteLine("Invalid key. Please try again.");
                                continue;
                        }

                        inSubMenu = true;
                        typeof(Program).GetMethod("PerformCrudOperation", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                                                .MakeGenericMethod(selectedTableType)
                                                .Invoke(null, new object[] { context });
                        inSubMenu = false;
                    }


                }
            }
        }
        private static void PerformCrudOperation<T>(DbContext context) where T : class
        {
            var table = context.Set<T>();
            var elements = table.ToList();
            int currentPosition = 0;

            while (true)
            {
                Console.Clear();
                if (elements.Count == 0)
                {
                    Console.WriteLine("No entries available.");
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                    return;
                }

                var pkName = context.GetPrimaryKeyName<T>(); // Get primary key name dynamically

                // Print current entry details
                PrintEntityDetails(elements[currentPosition], pkName);

                Console.WriteLine("\nOptions: \nPress 'u' to move up.\nPress 'd' to move down.\nPress 'v' to view all.\nPress 'r' to remove this entry.\nPress 'Esc' to quit.");
                var key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.U:
                        if (currentPosition > 0)
                        {
                            currentPosition--;
                        }
                        else
                        {
                            Console.WriteLine("This is already the first entry");
                            Console.ReadKey();
                        }
                        break;
                    case ConsoleKey.D:
                        if (currentPosition < (elements.Count - 1))
                        {
                            currentPosition++;
                        }
                        else
                        {
                            Console.WriteLine("This is already the last entry");
                            Console.ReadKey();
                        }
                        break;
                    case ConsoleKey.V:
                        Console.Clear();
                        Console.WriteLine("All Entries: ");
                        foreach (var element in elements)
                        {
                            PrintEntityDetails(element, pkName);
                        }
                        Console.WriteLine("\nPress Esc to return to menu");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.R:
                        var entity = elements[currentPosition];
                        context.Entry(entity).State = EntityState.Deleted;
                        context.SaveChanges();
                        elements.RemoveAt(currentPosition);
                        Console.WriteLine("\nEntry Deleted!");
                        if (currentPosition >= elements.Count)
                        {
                            currentPosition = elements.Count - 1;
                        }
                        Console.WriteLine("\nPress Esc to return to menu");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        Console.WriteLine("\n Invalid Option! Try Again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void PrintEntityDetails<T>(T entity, string pkName) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            Console.WriteLine($"Details of {typeof(T).Name}:");
            foreach (var prop in properties)
            {
                if (prop.Name == pkName) // Print primary key value
                {
                    var pkValue = prop.GetValue(entity);
                    Console.WriteLine($"\t{pkName}: {pkValue}");
                }
                else // Print other properties
                {
                    var value = prop.GetValue(entity);
                    Console.WriteLine($"\t{prop.Name}: {value}");
                }
            }
        }



    }
}





