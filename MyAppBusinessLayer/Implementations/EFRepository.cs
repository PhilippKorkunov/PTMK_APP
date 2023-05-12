using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using MyAppDbLayer;
using MyAppDbLayer.Entities.Abstractions;
using System.Linq.Expressions;

namespace MyAppBusinessLayer.Implementations
{
    public class EFRepository<T> : AbstractRepository
        where T : Entity
    {
        public EFRepository(MyAppEFDbContext myAppEFDbContext) : base(myAppEFDbContext) { }

        private static void CheckEntry(T entry) { if (entry is null) throw new ArgumentNullException(nameof(entry), $"{entry} must be non null"); }


        // CRUD для таблиц из базы данных как синхронные, так и асинхронные

        public void Delete(T entry)
        {
            CheckEntry(entry);
            myAppEFDbContext.Remove(entry);
            myAppEFDbContext.SaveChanges();
        }
        public async Task DeleteAsync(T entry)
        {
            CheckEntry(entry);
            myAppEFDbContext.Remove(entry);
            await myAppEFDbContext.SaveChangesAsync();
        }

        public void Update(T entry)
        {
            CheckEntry(entry);
            myAppEFDbContext.Update(entry);
            myAppEFDbContext.SaveChanges();
        }
        public async Task UpdateAsync(T entry)
        {
            CheckEntry(entry);
            myAppEFDbContext.Update(entry);
            await myAppEFDbContext.SaveChangesAsync();

        }

        public void Insert(T entry)
        {
            CheckEntry(entry);
            myAppEFDbContext.Add(entry);
            myAppEFDbContext.SaveChanges();
        }
        public async Task InsertAsync(T entry)
        {
            CheckEntry(entry);
            await myAppEFDbContext.AddAsync(entry);
            await myAppEFDbContext.SaveChangesAsync();
        }
        public void InsertRange(params T[] entries) //EF Core + BULK SQL
        {
            using (var transaction = myAppEFDbContext.Database.BeginTransaction())
            {
                myAppEFDbContext.BulkInsert(entries);
                transaction.Commit();
            }
        }
        public async Task InsertRangeAsync(params T[] entries) //EF Core + BULK SQL
        {
            using (var transaction = myAppEFDbContext.Database.BeginTransaction())
            {
                myAppEFDbContext.BulkInsert(entries);
                await transaction.CommitAsync();
            }
        }

        public IQueryable<T>? Get(int takeNumber = 0, Expression<Func<T, bool>>? predicate = null) 
        {
            if (takeNumber == 0) takeNumber = myAppEFDbContext.Set<T>().Count();

            return  predicate is null ? myAppEFDbContext.Set<T>().Take(takeNumber).ToList().AsQueryable() :
                                myAppEFDbContext.Set<T>().Take(takeNumber).ToList().AsQueryable().Where(predicate); 
        }
       
        public IQueryable<T>? Get(string sql) => myAppEFDbContext.Set<T>().FromSqlRaw(sql).ToList().AsQueryable();


        public async Task<IQueryable<T>?> GetAsync(int takeNumber = 0, Expression<Func<T, bool>>? predicate = null)
        {
            if (takeNumber == 0) takeNumber = await myAppEFDbContext.Set<T>().CountAsync();

            return predicate is null ? (await myAppEFDbContext.Set<T>().Take(takeNumber).ToListAsync()).AsQueryable() :
                    (await myAppEFDbContext.Set<T>().Where(predicate).Take(takeNumber).ToListAsync()).AsQueryable();
        }

        public async Task<IQueryable<T>?> GetAsync(string sql) => (await myAppEFDbContext.Set<T>().FromSqlRaw(sql).ToListAsync()).AsQueryable();

    }
}
