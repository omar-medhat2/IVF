using Northwind_EF;
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
    public class CategoryBus
    {

        public static void PrintData(Category element, int productPosition)
        {
            CategoryVM vm = new CategoryVM
            {
                CategoryID = element.CategoryID,
                CategoryName = element.CategoryName,
                Description = element.Description,
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
                var product = vm.Products.ElementAt(productPosition);
                string productInfo = $"\nProduct ID: {product.ProductID}\nProduct Name: {product.ProductName}\nQuantity Per Unit: {product.QuantityPerUnit}\nUnit Price: {product.UnitPrice}\nUnits In Stock: {product.UnitsInStock}";
                Console.WriteLine($"Category ID: {vm.CategoryID}\nCategory Name: {vm.CategoryName}\nDescription: {vm.Description}\nProduct Info: {productInfo}");
            }
            else
            {
                Console.WriteLine($"Category ID: {vm.CategoryID}\nCategory Name: {vm.CategoryName}\nDescription: {vm.Description}\nNo products available for this category.");
            }
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Categories.Include(c => c.Products).ToList();
                int currentCategoryPosition = 0;
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

                    var element = elements.ElementAt(currentCategoryPosition);

                    PrintData(element, currentProductPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'A' to move to the previous product.\nPress 'D' to move to the next product.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentCategoryPosition = (currentCategoryPosition > 0) ? currentCategoryPosition - 1 : currentCategoryPosition;
                            currentProductPosition = 0; // Reset product position when changing categories
                            break;
                        case ConsoleKey.S:
                            currentCategoryPosition = (currentCategoryPosition < elements.Count - 1) ? currentCategoryPosition + 1 : currentCategoryPosition;
                            currentProductPosition = 0; // Reset product position when changing categories
                            break;
                        case ConsoleKey.A:
                            currentProductPosition = (currentProductPosition > 0) ? currentProductPosition - 1 : currentProductPosition;
                            break;
                        case ConsoleKey.D:
                            currentProductPosition = (currentProductPosition < element.Products.Count - 1) ? currentProductPosition + 1 : currentProductPosition;
                            break;
                        case ConsoleKey.X:
                            context.Categories.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentCategoryPosition);
                            currentCategoryPosition = (currentCategoryPosition >= elements.Count) ? elements.Count - 1 : currentCategoryPosition;
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

