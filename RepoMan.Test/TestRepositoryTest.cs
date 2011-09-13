using Xunit;
using Rhino.Mocks;
using System.Linq;
using System.Collections.Generic;
using RepoMan.Test.Data;
using System.Data.Entity;

namespace RepoMan.Test
{
    class TestRepositoryTest
    {
        private TestRepository<RepoTestDatabaseEntities, Person> _subject;

        public TestRepositoryTest()
        {
            _subject = new TestRepository<RepoTestDatabaseEntities, Person>();
        }

        [Fact]
        public void where_should_return_an_iqueryable_list_of_TRepository()
        {
            Assert.IsType(typeof(IQueryable<int>), _subject.Where(person => true));
        }
    }
}
