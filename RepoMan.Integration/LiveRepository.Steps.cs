﻿using System;
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
        LiveRepository<RepoTestDatabaseEntities> _subject = new LiveRepository<RepoTestDatabaseEntities>();
        IQueryable<Person> _people;

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
    }
}