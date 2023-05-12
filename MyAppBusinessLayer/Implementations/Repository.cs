using Microsoft.EntityFrameworkCore;
using MyAppDbLayer;

namespace MyAppBusinessLayer.Implementations
{
    public class Repository : AbstractRepository
    {
        public Repository(MyAppEFDbContext myAppEFDbContext) : base(myAppEFDbContext)
        {}

        public int ExecuteNonQuery(string sql) => myAppEFDbContext.Database.ExecuteSqlRaw(sql);
        public async Task<int> ExexuteNonQueryAsync(string sql) => await myAppEFDbContext.Database.ExecuteSqlRawAsync(sql); //Воплнение sql-запроса
    }
}
