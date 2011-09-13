namespace RepoMan
{
    class Repository<TContext, TRepository> where TContext : IRepositoryContainer, new()
    {
        public TContext Context { get; set; }

        public System.Linq.IQueryable<TRepository> Query()
        {
            throw new System.NotImplementedException();
        }

        public void Save(TRepository entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(TRepository entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
