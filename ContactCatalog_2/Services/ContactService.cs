using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactCatalog_2.Repositories;
using ContactCatalog_2.Models;
using ContactCatalog_2.Validators;

namespace ContactCatalog_2.Services
{
    public class ContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public void SaveContact()
        {
            try
            {
                Console.Write("Enter ID: ");
                int idToBeAdded = int.Parse(Console.ReadLine());

                Console.Write("Enter Name: ");
                string nameToBeAdded = Console.ReadLine();

                Console.Write("Enter Email: ");
                string emailToBeAdded = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nameToBeAdded) || string.IsNullOrWhiteSpace(emailToBeAdded))
                {
                    throw new InvalidInputException("Name and/or Email cannot be empty.");
                }
                else if (!EmailValidator.IsValidEmail(emailToBeAdded))
                {
                    throw new InvalidInputException("Invalid email format.");
                }
                else
                {
                    Console.Write("Enter Tag: ");
                    string tagToBeAdded = Console.ReadLine();

                    var tags = new List<string> { tagToBeAdded };
                    var contactToBeAdded = new Contact(idToBeAdded, nameToBeAdded, emailToBeAdded, tags);

                    _repository.SaveContact(contactToBeAdded);

                    Console.WriteLine($"Contact {nameToBeAdded} is added.");
                }


            }
            catch (DuplicateEmailException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Input Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        public void RemoveContact()
        {
            try
            {
                Console.Write("Enter ID to remove: ");
                int idToBeRemoved = int.Parse(Console.ReadLine());

                _repository.RemoveContact(idToBeRemoved);
                Console.WriteLine($"Contact with ID {idToBeRemoved} is remooooved.");
            }
            catch (ContactNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        public void UpdateContact()
        {
            try
            {
                Console.Write("Enter contact ID to update: ");
                int idToBeUpdated = int.Parse(Console.ReadLine());

                _repository.UpdateContact(idToBeUpdated);
            }
            catch (ContactNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (DuplicateEmailException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Input Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        public void ListContacts()
        {
            var contacts = _repository.ListContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine("Chirp.Chirp.Chirp-There is nothing here.");
            }
            else
            {
                foreach (var human in contacts)
                {
                    Console.WriteLine($"ID: {human.Id}, Name: {human.Name}, Email: {human.Email}, Tags: {string.Join(", ", human.Tags)}");
                }
            }
        }

        public void SearchByName()
        {
            Console.Write("Enter name to search: ");
            string nameToSearch = Console.ReadLine();

            var results = _repository.SearchByName(nameToSearch);

            if (results.Count == 0)
            {
                Console.WriteLine("It echoes cause there is nothing here.");
            }
            else
            {
                foreach (var person in results)
                {
                    Console.WriteLine($"ID: {person.Id}, Name: {person.Name}, Email: {person.Email}, Tags: {string.Join(", ", person.Tags)}");
                }
            }
        }

        public void FilterByTag()
        {
            Console.Write("Enter tag to filter: ");
            string tagToFilter = Console.ReadLine();

            var results = _repository.FilterByTag(tagToFilter);

            if (results.Count == 0)
            {
                Console.WriteLine("Cricket sounds. No one here.");
            }
            else
            {
                foreach (var contact in results)
                {
                    Console.WriteLine($"ID: {contact.Id}, Name: {contact.Name}, Email: {contact.Email}, Tags: {string.Join(", ", contact.Tags)}");
                }
            }
        }
    }
}
