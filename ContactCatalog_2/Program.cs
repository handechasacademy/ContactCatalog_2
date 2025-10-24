using ContactCatalog_2.Models;
using ContactCatalog_2.Repositories;
using ContactCatalog_2.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;


namespace ContactCatalog_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            ILogger<ContactRepository> logger = loggerFactory.CreateLogger<ContactRepository>();

            var contacts = new Dictionary<int, Contact>();
            var emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var repository = new ContactRepository(contacts, emails, logger);
            var service = new ContactService(repository);

            bool running = true;

            while (running)
            {
                Console.WriteLine("CONTACT CATALOG MENU");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Remove Contact");
                Console.WriteLine("3. Update Contact");
                Console.WriteLine("4. List Contacts");
                Console.WriteLine("5. Search by Name");
                Console.WriteLine("6. Filter by Tag");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option (1–7): ");

                string choiceInput = Console.ReadLine();
                Console.WriteLine();

                switch (choiceInput)
                {
                    case "1":
                        service.SaveContact();
                        break;
                    case "2":
                        service.RemoveContact();
                        break;
                    case "3":
                        service.UpdateContact();
                        break;
                    case "4":
                        service.ListContacts();
                        break;
                    case "5":
                        service.SearchByName();
                        break;
                    case "6":
                        service.FilterByTag();
                        break;
                    case "7":
                        running = false;
                        Console.WriteLine("Exiting Contact Catalog. Goodbye! Come Again!!!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose between 1–7.");
                        break;
                }
            }
        }
    }
}
