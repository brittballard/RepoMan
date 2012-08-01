using Xunit;
using Rhino.Mocks;
using System.Linq;
using System.Collections.Generic;
using RepoMan.Test.Data;
using System.Data.Entity;

namespace RepoMan.Test
{
    public class TestRepositoryTest
    {
        private TestRepository<RepoTestDatabaseEntities> _subject;

        public TestRepositoryTest()
        {
            _subject = new TestRepository<RepoTestDatabaseEntities>();
        }

        [Fact]
        public void where_should_return_an_iqueryable_list_of_TRepository()
        {
            _subject.InitializeRepository<Person>();
            Assert.IsType(typeof(EnumerableQuery<Person>), _subject.Where<Person>(person => true));
        }

        [Fact]
        public void save_should_add_a_TRepository_to_the_list_correct_repository()
        {
            _subject.Save(new Person() { FirstName = "Britton" });
            var people = _subject.Where<Person>(person => true);

            Assert.Equal(1, people.Count());
        }

        [Fact]
        public void save_should_add_a_TRepository_to_the_list_correct_repository_even_when_there_is_already_a_repository_for_the_given_type()
        {
            _subject.Save(new Person() { FirstName = "Britton" });
            _subject.Save(new Person() { FirstName = "Cassie" });
            var people = _subject.Where<Person>(person => true);

            Assert.Equal(2, people.Count());
        }

        [Fact]
        public void where_should_return_an_iqueryable_list_of_TRepository_but_only_those_that_match_the_query_provided()
        {
            _subject.Save(new Person() { FirstName = "Britton" });
            _subject.Save(new Person() { FirstName = "Cassie" });
            var people = _subject.Where<Person>(person => person.FirstName == "Cassie");

            Assert.Equal(1, people.Count());
            Assert.Equal("Cassie", people.First().FirstName);
        }

        [Fact]
        public void delete_should_remove_the_object_provided_as_an_argument()
        {
            var personToDelete = new Person() { FirstName = "Britton" };
            _subject.Save(personToDelete);
            _subject.Save(new Person() { FirstName = "Cassie" });
            _subject.Delete(personToDelete);
            var people = _subject.Where<Person>(person => true);

            Assert.Equal(1, people.Count());
            Assert.Equal("Cassie", people.First().FirstName);
        }

        [Fact]
        public void firstordefault_should_return_first_matching_result()
        {
            _subject.Save(new Person() { FirstName = "Britton" });
            _subject.Save(new Person() { FirstName = "Cassie" });
            var person = _subject.FirstOrDefault<Person>(x => x.FirstName == "Britton");

            Assert.Equal("Britton", person.FirstName);
        }

        [Fact]
        public void firstordefault_should_return_null_if_no_match_exists()
        {
            _subject.Save(new Person() { FirstName = "Britton" });
            _subject.Save(new Person() { FirstName = "Cassie" });
            var person = _subject.FirstOrDefault<Person>(x => x.FirstName == "SomeoneElse");

            Assert.Null(person);
        }

        [Fact]
        public void where_with_columns_should_return_only_selected_columns()
        {
            _subject.Save(new Person() { Id = 1, FirstName = "Britton" });
            var person = _subject.FirstOrDefault<Person>(x => x.Id == 1, x => new {x.Id});

            Assert.Equal(1, person.Id);
            Assert.Null(person.FirstName);
        }
    }
}
