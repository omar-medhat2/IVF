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
    public class CustomerDemoBus
    {
            public static void PrintData(CustomerDemographic element, int customerPosition)
            {
                CustomerDemographicVM vm = new CustomerDemographicVM
                {
                    CustomerTypeID = element.CustomerTypeID,
                    CustomerDesc = element.CustomerDesc,
                    Customers = element.Customers.Select(o => new CustomerVM
                    {
                        CustomerID = o.CustomerID,
                        CompanyName = o.CompanyName
                    }).ToList()
                };

                if (vm.Customers.Any())
                {
                    var customer = vm.Customers.ElementAt(customerPosition);
                    string customerInfo = $"\nCustomer ID: {customer.CustomerID}\nCompany Name: {customer.CompanyName}";
                    Console.WriteLine($"Customer Type ID: {vm.CustomerTypeID}\nCustomerDesc: {vm.CustomerDesc}" +  "\n" + customerInfo);
                }
                else
                {
                    Console.WriteLine($"Customer Type ID: {vm.CustomerTypeID}\nCustomerDesc: {vm.CustomerDesc}\nNo customers available for this demographic.");
                }
            }

            public static void PerformCRUD()
            {
                using (var context = new NorthwindEntities())
                {
                    var elements = context.CustomerDemographics.Include(s => s.Customers).ToList(); // Include Orders to ensure they are loaded
                    int currentDemographicPosition = 0;
                    int currentCustomerPosition = 0;
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

                        var element = elements.ElementAt(currentDemographicPosition);

                        PrintData(element, currentCustomerPosition);

                        Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'A' to move to the previous order.\nPress 'D' to move to the next order.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                        var key = Console.ReadKey(intercept: true).Key;
                        switch (key)
                        {
                            case ConsoleKey.W:
                                currentDemographicPosition = (currentDemographicPosition > 0) ? currentDemographicPosition - 1 : currentDemographicPosition;
                                currentCustomerPosition = 0; // Reset order position when changing shippers
                                break;
                            case ConsoleKey.S:
                                currentDemographicPosition = (currentDemographicPosition < elements.Count - 1) ? currentDemographicPosition + 1 : currentDemographicPosition;
                                currentCustomerPosition = 0; // Reset order position when changing shippers
                                break;
                            case ConsoleKey.A:
                                currentCustomerPosition = (currentCustomerPosition > 0) ? currentCustomerPosition - 1 : currentCustomerPosition;
                                break;
                            case ConsoleKey.D:
                                currentCustomerPosition = (currentCustomerPosition < element.Customers.Count - 1) ? currentCustomerPosition + 1 : currentCustomerPosition;
                                break;
                            case ConsoleKey.X:
                                context.CustomerDemographics.Remove(element);
                                context.SaveChanges();
                                elements.RemoveAt(currentDemographicPosition);
                                currentDemographicPosition = (currentDemographicPosition >= elements.Count) ? elements.Count - 1 : currentDemographicPosition;
                                currentCustomerPosition = 0; // Reset order position when changing shippers
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
