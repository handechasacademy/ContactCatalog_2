using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactCatalog_2.Models;

namespace ContactCatalog_2.Repositories
{
    public interface IContactRepository
    {
        void SaveContact(Contact contact);
        void RemoveContact(int id);
        List<Contact> SearchByName(string namePart);
        List<Contact> FilterByTag(string tag);
        List<Contact> ListContacts();
        void UpdateContact(int id);
    }
}
