using ContactCatalog_2.Models;
using ContactCatalog_2.Repositories;
using Xunit;


namespace ContactCatalog_2.Tests
{
    public class ContactRepositoryTest
    {
        [Fact]
        public void SearchByName_ShouldReturnMatchingContacts()
        {
            //Arrange
            var repo = new ContactRepository(new Dictionary<int, Contact>(), new HashSet<string>(), null);
            var contact1 = new Contact(1, "Gimli", "gimli@dwarf.com", new List<string> { "Fellow" });
            var contact2 = new Contact(2, "Saruman", "saruman@wizard.com", new List<string> { "Villain" });
            repo.SaveContact(contact1);
            repo.SaveContact(contact2);

            //Act
            var results = repo.SearchByName("Gimli");
            //Assert           
            Assert.Contains(results, a => a.Name == "Gimli");
        }

        public void FilterByTag_ShouldReturnContactsWithTag()
        {
            //Arrange
            var repo = new ContactRepository(new Dictionary<int, Contact>(), new HashSet<string>(), null);
            var contact1 = new Contact(1, "Merry", "merry@hobbit.com", new List<string> { "Fellow" });
            var contact2 = new Contact(2, "Sauron", "sauron@maia.com", new List<string> { "Villain" });
            repo.SaveContact(contact1);
            repo.SaveContact(contact2);

            //Act
            var results = repo.FilterByTag("fellow");

            //Assert
            Assert.Contains(results, b => b.Name == "Merry");
        }

        [Fact]
        public void ListContacts_ShouldReturnAllContactsOrderedById()
        {
            //Arrange
            var repo = new ContactRepository(new Dictionary<int, Contact>(), new HashSet<string>(), null);
            var contact1 = new Contact(2, "Hurin", "hurin@human.com", new List<string> { "Hero" });
            var contact2 = new Contact(1, "Morgoth", "morgoth@valar.com", new List<string> { "Villain" });
            repo.SaveContact(contact1);
            repo.SaveContact(contact2);

            //Act
            var results = repo.ListContacts();

            //Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(1, results[0].Id);
            Assert.Equal(2, results[1].Id);
        }
    }
}