using Northwind_EF.VMs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.Businesses
{
    public class ProductBus
    {

        public static void PrintData(Product element, int customerPosition)
        {
            var vm = new ProductVM
            {
                ProductID = element.ProductID,
                ProductName = element.ProductName,
                QuantityPerUnit = element.QuantityPerUnit,
                UnitPrice = element.UnitPrice,
                UnitsInStock = element.UnitsInStock,
                CategoryID = element.CategoryID,
                SupplierID = element.SupplierID,
                Supplier = element.Supplier != null ? new SupplierVM
                {
                    SupplierID = element.Supplier.SupplierID,
                    CompanyName = element.Supplier.CompanyName,
                    ContactName = element.Supplier.ContactName,
                    Phone = element.Supplier.Phone,
                } : null,
                Category = element.Category != null ? new CategoryVM
                {
                    CategoryID = element.Category.CategoryID,
                    CategoryName = element.Category.CategoryName,
                } : null
            };

            string product = $"\nProduct ID: {vm.ProductID}\nProduct Name: {vm.ProductName}\nQuantity Per Unit: {vm.QuantityPerUnit}\nUnit Price: {vm.UnitPrice}\nUnits In Stock: {vm.UnitsInStock}";

            if (vm.Supplier != null)
            {
                string supplierInfo = $"\nSupplier ID: {vm.Supplier.SupplierID}\nSupplier Name: {vm.Supplier.CompanyName}\nContact Name: {vm.Supplier.ContactName}\nPhone: {vm.Supplier.Phone}";
                product += supplierInfo;
            }

            if (vm.Category != null)
            {
                string categoryInfo = $"\nCategory ID: {vm.Category.CategoryID}\nCategory Name: {vm.Category.CategoryName}";
                product += categoryInfo;
            }

            Console.WriteLine(product);
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Products.Include(p => p.Supplier).Include(p => p.Category).ToList(); 
                int currentProductPosition = 0;
                bool flag = true;

                while (flag)
                {
                    Console.Clear();
                    if (!elements.Any())
                    {
                        Console.WriteLine("No entries available.");
                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                        flag = false;
                        break;
                    }

                    var element = elements.ElementAt(currentProductPosition);
                    PrintData(element, currentProductPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentProductPosition = (currentProductPosition > 0) ? currentProductPosition - 1 : currentProductPosition;
                            break;
                        case ConsoleKey.S:
                            currentProductPosition = (currentProductPosition < elements.Count - 1) ? currentProductPosition + 1 : currentProductPosition;
                            break;
                        case ConsoleKey.X:
                            context.Products.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentProductPosition);
                            currentProductPosition = (currentProductPosition >= elements.Count) ? elements.Count - 1 : currentProductPosition;
                            Console.WriteLine("\nEntry Deleted!\nPress any key to return to menu");
                            Console.ReadKey();
                            break;
                        case ConsoleKey.N:
                            Console.Clear();
                            // Add new entry logic
                            break;
                        case ConsoleKey.U:
                            Console.Clear();
                            // Update entry logic
                            break;
                        case ConsoleKey.Escape:
                            flag = false;
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("\n Invalid Option! Try Again.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            return;
        }
    }

}
