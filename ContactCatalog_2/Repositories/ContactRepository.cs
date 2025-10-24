using System;
using System.Collections.Generic;
using System.Linq;
using ContactCatalog_2.Validators;
using ContactCatalog_2.Models;
using ContactCatalog_2.Services;
using Microsoft.Extensions.Logging;

namespace ContactCatalog_2.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private Dictionary<int, Contact> _contacts = new Dictionary<int, Contact>();
        private HashSet<string> _emails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private ILogger<ContactRepository> _logger;


        public ContactRepository(Dictionary<int, Contact> contacts,
                         HashSet<string> emails,
                         ILogger<ContactRepository> logger)
        {
            _contacts = contacts;
            _emails = emails;
            _logger = logger;
        }

        public void SaveContact(Contact contact)
        {
            if (_contacts.ContainsKey(contact.Id))
            {
                throw new Exception("ID already exists.");
            }
            else if (_emails.Contains(contact.Email))
            {
                throw new Exception("Email already exists.");
            }
            else
            {
                _contacts.Add(contact.Id, contact);
                _emails.Add(contact.Email);
            }
        }

        public void UpdateContact(int id)
        {
            if (_contacts.ContainsKey(id))
            {
                var contact = _contacts[id];
                Console.WriteLine($"Updating contact: {contact.Name} ({contact.Email})");

                Console.Write("Enter new name (leave empty to keep it as it is): ");
                string nameToBeUpdated = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nameToBeUpdated))
                {
                    contact.Name = nameToBeUpdated;
                }

                Console.Write("Enter new email (leave empty to keep it as it is): ");
                string emailToBeUpdated = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(emailToBeUpdated))
                {
                    if (_emails.Contains(emailToBeUpdated))
                    {
                        throw new DuplicateEmailException(emailToBeUpdated);
                    }
                    else if (!EmailValidator.IsValidEmail(emailToBeUpdated))
                    {
                        throw new InvalidInputException("Invalid email format.");
                    }
                    else
                    {
                        _emails.Remove(contact.Email);
                        contact.Email = emailToBeUpdated;
                        _emails.Add(emailToBeUpdated);
                    }
                }

                Console.Write("Enter tag to add (leave empty to skip): ");
                string tagToBeAdded = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(tagToBeAdded))
                {
                    if (contact.Tags.Contains(tagToBeAdded, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new InvalidInputException($"Tag '{tagToBeAdded}' already exists for this contact.");
                    }
                    else
                    {
                        contact.Tags.Add(tagToBeAdded);
                        Console.WriteLine($"Tag '{tagToBeAdded}' added.");
                    }
                }

                Console.Write("Enter tag to remove (leave empty to skip): ");
                string tagToBeRemoved = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(tagToBeRemoved))
                {
                    if (contact.Tags.Contains(tagToBeRemoved, StringComparer.OrdinalIgnoreCase))
                    {
                        contact.Tags.Remove(tagToBeRemoved);
                    }
                    else
                    {
                        throw new InvalidInputException($"Tag '{tagToBeRemoved}' not found for this contact.");
                    }
                }

                Console.WriteLine("Contact updated.");
            }
            else
            {
                throw new ContactNotFoundException(id);
            }
        }


        public List<Contact> SearchByName(string namePart)
        {
            return _contacts.Values
                .Where(s => s.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Name)
                .ToList();
        }

        public List<Contact> FilterByTag(string tag)
        {
            return _contacts.Values
                .Where(f => f.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
                .OrderBy(f => f.Name)
                .ToList();
        }

        public void RemoveContact(int id)
        {
            if (_contacts.ContainsKey(id))
            {
                _emails.Remove(_contacts[id].Email);
                _contacts.Remove(id);
                Console.WriteLine($"Contact with ID {id} has been removed.");
            }
            else
            {
                throw new ContactNotFoundException(id);
            }
        }

        public List<Contact> ListContacts()
        {
            return _contacts.Values.OrderBy(l => l.Id).ToList();
        }
    }
}
