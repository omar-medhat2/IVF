using System;
using Northwind_EF.VMs;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Northwind_EF.Businesses
{
    internal class RegionBus
    {
        public static void PrintData(Region element, int regionPosition)
        {
            var vm = new RegionVM
            {
                RegionID = element.RegionID,
                RegionDescription = element.RegionDescription,
                Territories = element.Territories?.Select(t => new TerritoryVM
                {
                    TerritoryID = t.TerritoryID,
                    TerritoryDescription = t.TerritoryDescription
                }).ToList()
            };

            string region = $"\nRegion ID: {vm.RegionID}\nRegion Description: {vm.RegionDescription}";

            if (vm.Territories != null && vm.Territories.Any())
            {
                string territoriesInfo = "\nTerritories:";
                foreach (var territory in vm.Territories)
                {
                    territoriesInfo += $"\n - Territory ID: {territory.TerritoryID}, Territory Description: {territory.TerritoryDescription}";
                }
                region += territoriesInfo;
            }

            Console.WriteLine(region);
        }

        public static void PerformCRUD()
        {
            using (var context = new NorthwindEntities())
            {
                var elements = context.Regions.Include(r => r.Territories).ToList();
                int currentRegionPosition = 0;
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

                    var element = elements.ElementAt(currentRegionPosition);
                    PrintData(element, currentRegionPosition);

                    Console.WriteLine("\nOptions: \nPress 'W' to move up.\nPress 'S' to move down.\nPress 'X' to remove this entry.\nPress 'N' to add a new entry.\nPress 'U' to update an existing entry.\nPress 'Esc' to quit.");
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            currentRegionPosition = (currentRegionPosition > 0) ? currentRegionPosition - 1 : currentRegionPosition;
                            break;
                        case ConsoleKey.S:
                            currentRegionPosition = (currentRegionPosition < elements.Count - 1) ? currentRegionPosition + 1 : currentRegionPosition;
                            break;
                        case ConsoleKey.X:
                            context.Regions.Remove(element);
                            context.SaveChanges();
                            elements.RemoveAt(currentRegionPosition);
                            currentRegionPosition = (currentRegionPosition >= elements.Count) ? elements.Count - 1 : currentRegionPosition;
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