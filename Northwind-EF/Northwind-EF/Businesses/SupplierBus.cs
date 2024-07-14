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
    public class SupplierBus
    {
        public static void PrintData(Supplier element, int supplierPosition)
        {
            SupplierVM vm = new SupplierVM
            {
                SupplierID = element.SupplierID,
                CompanyName = element.CompanyName,
                ContactName = element.ContactName,
                Phone = element.Phone,
                Products = element.Products.Select(p => new ProductVM
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock
                }).ToList()
            };

            if (vm.Products.Any())
            {
                var product = vm.Products.ElementAt(supplierPosition);
                string productInfo = $"\nProduct ID: {product.ProductID}\nProduct Name: {product.ProductName}\nQuantity Per Unit: {product.QuantityPerUnit}\nUnit Price: {product.UnitPrice}\nUnits In Stock: {product.UnitsInStock}";
                Console.WriteLine($"Supplier ID: {vm.SupplierID}\nCompany Name: {vm.CompanyName}\nContact Name: {vm.ContactName}\nPhone: {vm.Phone}\nProduct Info: {productInfo}");
            }
            else
            {
                Console.WriteLine($"Supplier ID: {vm.SupplierID}\nCompany Name: {vm.CompanyName}\nContact Name: {vm.ContactName}\nPhone: {vm.Phone}\nNo products available for this category.");
            }
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Suppliers.Include(c => c.Products).ToList();
                int currentSupplierPosition = 0;
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

                    var element = elements.ElementAt(currentSupplierPosition);

                    PrintData(element, currentProductPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'A' to move to the previous product.\nPress 'D' to move to the next product.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentSupplierPosition = (currentSupplierPosition > 0) ? currentSupplierPosition - 1 : currentSupplierPosition;
                            currentProductPosition = 0; // Reset product position when changing categories
                            break;
                        case ConsoleKey.S:
                            currentSupplierPosition = (currentSupplierPosition < elements.Count - 1) ? currentSupplierPosition + 1 : currentSupplierPosition;
                            currentProductPosition = 0; // Reset product position when changing categories
                            break;
                        case ConsoleKey.A:
                            currentProductPosition = (currentProductPosition > 0) ? currentProductPosition - 1 : currentProductPosition;
                            break;
                        case ConsoleKey.D:
                            currentProductPosition = (currentProductPosition < element.Products.Count - 1) ? currentProductPosition + 1 : currentProductPosition;
                            break;
                        case ConsoleKey.X:
                            context.Suppliers.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentSupplierPosition);
                            currentSupplierPosition = (currentSupplierPosition >= elements.Count) ? elements.Count - 1 : currentSupplierPosition;
                            currentProductPosition = 0; // Reset product position when changing categories
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
