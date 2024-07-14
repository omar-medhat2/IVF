
using Northwind_EF.Businesses;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;


namespace Northwind_EF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Northwind_EF.NorthwindEntities context = new NorthwindEntities();

            using (context = new NorthwindEntities())
            {
                Console.WriteLine("Welcome, Admin!\n");
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
                        Console.WriteLine("Press 'Esc' twice to quit.");
                    }

                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // intercept: true to prevent the key from being shown in the console

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        exit = true;
                        continue;
                    }

                    if (!inSubMenu)
                    {
                        

                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.D0:
                            case ConsoleKey.NumPad0:
                                selectedTable = context.CustomerDemographics;
                                Console.WriteLine("CustomerDemographic table selected.");
                                CustomerDemoBus.PerformCRUD();
                                break;
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:
                                selectedTable = context.Customers;
                                Console.WriteLine("Customer table selected.");
                                CustomerBus.PerformCRUD();
                                break;
                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2:
                                selectedTable = context.Orders;
                                Console.WriteLine("Order table selected.");
                                OrderBus.PerformCRUD();
                                break;
                            case ConsoleKey.D3:
                            case ConsoleKey.NumPad3:
                                selectedTable = context.Regions;
                                Console.WriteLine("Region table selected.");
                                RegionBus.PerformCRUD();
                                break;
                            case ConsoleKey.D4:
                            case ConsoleKey.NumPad4:
                                selectedTable = context.Employees;
                                Console.WriteLine("Employee table selected.");
                                EmployeeBus.PerformCRUD();
                                break;
                            case ConsoleKey.D5:
                            case ConsoleKey.NumPad5:
                                selectedTable = context.Territories;
                                Console.WriteLine("Territory table selected.");
                                TerritoryBus.PerformCRUD();
                                break;
                            case ConsoleKey.D6:
                            case ConsoleKey.NumPad6:
                                selectedTable = context.Shippers;
                                Console.WriteLine("Shipper table selected.");
                                ShipperBus.PerformCRUD();
                                break;
                            case ConsoleKey.D7:
                            case ConsoleKey.NumPad7:
                                selectedTable = context.Suppliers;
                                Console.WriteLine("Supplier table selected.");
                                SupplierBus.PerformCRUD();
                                break;
                            case ConsoleKey.D8:
                            case ConsoleKey.NumPad8:
                                selectedTable = context.Categories;
                                Console.WriteLine("Category table selected.");
                                CategoryBus.PerformCRUD();
                                break;
                            case ConsoleKey.D9:
                            case ConsoleKey.NumPad9:
                                selectedTable = context.Products;
                                Console.WriteLine("Product table selected.");
                                ProductBus.PerformCRUD();
                                break;
                            default:
                                Console.WriteLine("Invalid key. Please try again.");
                                continue;
                        }

                        inSubMenu = true;
                        inSubMenu = false;
                    }


                }
            }
        }
       

       



    }
}





