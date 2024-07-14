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
    internal class EmployeeBus
    {
        public static void PrintData(Employee element, int employeePosition)
        {
            var vm = new EmployeeVM
            {
                EmployeeID = element.EmployeeID,
                LastName = element.LastName,
                FirstName = element.FirstName,
                Title = element.Title,
                TitleOfCourtesy = element.TitleOfCourtesy,
                BirthDate = element.BirthDate,
                HireDate = element.HireDate,
                Address = element.Address,
                City = element.City,
                Region = element.Region,
                PostalCode = element.PostalCode,
                Country = element.Country,
                HomePhone = element.HomePhone,
                Extension = element.Extension,
                ReportsTo = element.ReportsTo,
                Employees1 = element.Employees1?.Select(e => new EmployeeVM
                {
                    EmployeeID = e.EmployeeID,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                }).ToList(),
                Employee1 = element.Employee1 != null ? new EmployeeVM
                {
                    EmployeeID = element.Employee1.EmployeeID,
                    FirstName = element.Employee1.FirstName,
                    LastName = element.Employee1.LastName
                } : null,
                Orders = element.Orders?.Select(o => new OrderVM
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    ShippedDate = o.ShippedDate,
                    ShipName = o.ShipName
                }).ToList(),
                Territories = element.Territories?.Select(t => new TerritoryVM
                {
                    TerritoryID = t.TerritoryID,
                    TerritoryDescription = t.TerritoryDescription
                }).ToList()
            };

            string employee = $"\nEmployee ID: {vm.EmployeeID}\nLast Name: {vm.LastName}\nFirst Name: {vm.FirstName}\nTitle: {vm.Title}\nTitle Of Courtesy: {vm.TitleOfCourtesy}\nBirth Date: {vm.BirthDate}\nHire Date: {vm.HireDate}\nAddress: {vm.Address}\nCity: {vm.City}\nRegion: {vm.Region}\nPostal Code: {vm.PostalCode}\nCountry: {vm.Country}\nHome Phone: {vm.HomePhone}\nExtension: {vm.Extension}\nReports To: {vm.ReportsTo}";

            if (vm.Employees1 != null && vm.Employees1.Any())
            {
                string subordinatesInfo = "\nSubordinates:";
                foreach (var subordinate in vm.Employees1)
                {
                    subordinatesInfo += $"\n - Employee ID: {subordinate.EmployeeID}, Name: {subordinate.FirstName} {subordinate.LastName}";
                }
                employee += subordinatesInfo;
            }

            if (vm.Employee1 != null)
            {
                string managerInfo = $"\nManager: {vm.Employee1.FirstName} {vm.Employee1.LastName}";
                employee += managerInfo;
            }

            if (vm.Orders != null && vm.Orders.Any())
            {
                string ordersInfo = "\nOrders:";
                foreach (var order in vm.Orders)
                {
                    ordersInfo += $"\n - Order ID: {order.OrderID}, Order Date: {order.OrderDate}, Shipped Date: {order.ShippedDate}, Ship Name: {order.ShipName}";
                }
                employee += ordersInfo;
            }

            if (vm.Territories != null && vm.Territories.Any())
            {
                string territoriesInfo = "\nTerritories:";
                foreach (var territory in vm.Territories)
                {
                    territoriesInfo += $"\n - Territory ID: {territory.TerritoryID}, Territory Description: {territory.TerritoryDescription}";
                }
                employee += territoriesInfo;
            }

            Console.WriteLine(employee);


        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Employees.Include(e => e.Employees1).Include(e => e.Employee1).Include(e => e.Orders).Include(e => e.Territories).ToList();
                int currentEmployeePosition = 0;
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

                    var element = elements.ElementAt(currentEmployeePosition);
                    PrintData(element, currentEmployeePosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentEmployeePosition = (currentEmployeePosition > 0) ? currentEmployeePosition - 1 : currentEmployeePosition;
                            break;
                        case ConsoleKey.S:
                            currentEmployeePosition = (currentEmployeePosition < elements.Count - 1) ? currentEmployeePosition + 1 : currentEmployeePosition;
                            break;
                        case ConsoleKey.X:
                            context.Employees.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentEmployeePosition);
                            currentEmployeePosition = (currentEmployeePosition >= elements.Count) ? elements.Count - 1 : currentEmployeePosition;
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
                            Console.Clear(); // Clear the screen to show the main menu
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
