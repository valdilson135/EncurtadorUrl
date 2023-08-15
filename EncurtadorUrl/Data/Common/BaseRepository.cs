namespace EncurtadorUrl.Data.Common
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext DbContext;
        public BaseRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
