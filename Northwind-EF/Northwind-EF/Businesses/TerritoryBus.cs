using System;
using Northwind_EF.VMs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Northwind_EF.Businesses
{
    internal class TerritoryBus
    {
        public static void PrintData(Territory element, int territoryPosition)
        {
            var vm = new TerritoryVM
            {
                TerritoryID = element.TerritoryID,
                TerritoryDescription = element.TerritoryDescription,
                RegionID = element.RegionID,
                Region = element.Region != null ? new RegionVM
                {
                    RegionID = element.Region.RegionID,
                    RegionDescription = element.Region.RegionDescription
                } : null,
                Employees = element.Employees?.Select(e => new EmployeeVM
                {
                    EmployeeID = e.EmployeeID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Title = e.Title
                }).ToList()
            };

            string territory = $"\nTerritory ID: {vm.TerritoryID}\nTerritory Description: {vm.TerritoryDescription}\nRegion ID: {vm.RegionID}";

            if (vm.Region != null)
            {
                string regionInfo = $"\nRegion Description: {vm.Region.RegionDescription}";
                territory += regionInfo;
            }

            if (vm.Employees != null && vm.Employees.Any())
            {
                string employeesInfo = "\nEmployees:";
                foreach (var employee in vm.Employees)
                {
                    employeesInfo += $"\n - Employee ID: {employee.EmployeeID}, Name: {employee.FirstName} {employee.LastName}, Title: {employee.Title}";
                }
                territory += employeesInfo;
            }

            Console.WriteLine(territory);
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Territories.Include(t => t.Region).Include(t => t.Employees).ToList();
                int currentTerritoryPosition = 0;
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

                    var element = elements.ElementAt(currentTerritoryPosition);
                    PrintData(element, currentTerritoryPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentTerritoryPosition = (currentTerritoryPosition > 0) ? currentTerritoryPosition - 1 : currentTerritoryPosition;
                            break;
                        case ConsoleKey.S:
                            currentTerritoryPosition = (currentTerritoryPosition < elements.Count - 1) ? currentTerritoryPosition + 1 : currentTerritoryPosition;
                            break;
                        case ConsoleKey.X:
                            context.Territories.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentTerritoryPosition);
                            currentTerritoryPosition = (currentTerritoryPosition >= elements.Count) ? elements.Count - 1 : currentTerritoryPosition;
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
                            return;
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
