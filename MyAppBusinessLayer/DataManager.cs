using MyAppBusinessLayer.Implementations;
using MyAppDbContext.Entities;
using MyAppDbLayer.Entities;

namespace MyAppBusinessLayer
{
    public class DataManager
    {
        public EFRepository<User> EfUserRepository { get; private set; }
        public Repository Repository { get; private set; }

        public DataManager(EFRepository<User> efUserRepository, Repository repository) 
        {
            this.Repository = repository;
            EfUserRepository = efUserRepository;
        }
    }
}
