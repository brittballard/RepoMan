using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using RepoMan.Test.Data;
using Xunit;

namespace RepoMan.Integration
{
    [Binding]
    public class LiveRepository
    {
        private LiveRepository<RepoTestDatabaseEntities> _subject = new LiveRepository<RepoTestDatabaseEntities>();
        private IQueryable<Person> _people;
        private Person _person;

        [Given(@"I have a Person entity in my database like:")]
        public void GivenIHaveAPersonEntityInMyDatabaseLike(Table table)
        {
            foreach (var person in table.Rows)
            {
                _subject.Save(new Person { FirstName = person["FirstName"] });
            }
        }

        [Then(@"I should find (\d+) (Person|People)")]
        public void ThenIShouldFind1Person(int numberOfPeople, string personOrPeople)
        {
            Assert.Equal(numberOfPeople, _people.Count());
        }

        [Then(@"the Person's name should be (\w+)")]
        public void ThenThePersonSNameShouldBeBritton(string nameOfPerson)
        {
            Assert.Equal(nameOfPerson, _people.First().FirstName);
        }

        [Then(@"The results should only have Id populated")]
        public void ThenTheResultsShouldOnlyHaveIdPopulated()
        {
            foreach (var person in _people)
            {
                Assert.True(person.Id > 0);
                Assert.Null(person.FirstName);
            }
        }

        [Then(@"The Person's name should be (\w+)")]
        public void ThenThePersonSNameShouldBeX(string firstName)
        {
            Assert.NotNull(_person);
            Assert.Equal(firstName, _person.FirstName);
        }

        [When(@"I query the Person repository")]
        public void WhenIQueryThePersonRepository()
        {
            _people = _subject.Where<Person>(person => true);
        }

        [When(@"I delete the Person from the repository")]
        public void WhenIDeleteThePersonFromTheRepository()
        {
            _subject.Delete(_subject.Where<Person>(person => true).First());
        }

        [When(@"I query the Person repository for only Id")]
        public void WhenIQueryThePersonRepositoryForOnlyId()
        {
            _people = _subject.Where<Person>(person => true, x => new {x.Id});
        }

        [When(@"I lookup the Person repository for the name (\w+)")]
        public void WhenILookupThePersonRepositoryForTheNameX(string firstName)
        {
            _person = _subject.FirstOrDefault<Person>(x => x.FirstName == firstName);
        }

    }
}
