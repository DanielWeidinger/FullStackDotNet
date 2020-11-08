using System;
using System.Linq;
using Devices.Core;
using Devices.Persistence;

namespace Devices.ConsoleApp
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine(">> Import der Usages in die Datenbank");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();

            await using UnitOfWork unitOfWorkImport = new UnitOfWork();
            Console.WriteLine("Datenbank löschen");
            await unitOfWorkImport.DeleteDatabaseAsync();
            Console.WriteLine("Datenbank migrieren");
            await unitOfWorkImport.MigrateDatabaseAsync();
            Console.WriteLine();

            Console.WriteLine("Messwerte werden von measurements.csv eingelesen");
            Console.WriteLine("------------------------------------------------");
            var usages = await ImportController.ReadFromCsvAsync();

            if (usages.Count() == 0)
            {
                Console.WriteLine("!!! Es wurden keine Messwerte eingelesen");
                return;
            }

            Console.WriteLine(
                $"  Es wurden {usages.Count()} Usages eingelesen, werden in Datenbank gespeichert ...");
            await unitOfWorkImport.UsageRepository.AddRangeAsync(usages);
            await unitOfWorkImport.SaveChangesAsync();
            await unitOfWorkImport.DisposeAsync();


            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }
    }
}