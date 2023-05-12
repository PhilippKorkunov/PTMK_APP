using MyAppDbLayer;

namespace MyAppBusinessLayer.Implementations
{
    public abstract class AbstractRepository :IAsyncDisposable
    {
        private protected readonly MyAppEFDbContext myAppEFDbContext;

        public AbstractRepository(MyAppEFDbContext myAppEFDbContext)
        {
            this.myAppEFDbContext = myAppEFDbContext;
        }

        public async ValueTask DisposeAsync()
        {
            await myAppEFDbContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
