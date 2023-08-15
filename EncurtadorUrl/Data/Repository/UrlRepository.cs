using EncurtadorUrl.Data.Common;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EncurtadorUrl.Data.Repository
{
    public class UrlRepository : BaseRepository, IUrlRepository
    {
        public UrlRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task CreateUrl(UrlModel url)
        {
              DbContext.Urls.Add(url);
                await SaveChanges();         
        }

        public async Task<IEnumerable<UrlModel>> GetAllUrls()
        {
            return await DbContext.Urls.ToListAsync();
        }

        public async Task<UrlModel> GetUrlById(int id)
        {
            return await DbContext.Urls.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UrlModel> GetUrlByUrl(UrlModel url)
        {
            return await DbContext.Urls.Where(x => x.Url.ToLower().Trim() == url.Url.ToLower().Trim()).FirstOrDefaultAsync();
        }
        public async Task<UrlModel> GetUrlByShortUrl(UrlModel url)
        {
            return await DbContext.Urls.Where(x => x.ShortUrl.ToLower().Trim() == url.ShortUrl.ToLower().Trim()).FirstOrDefaultAsync();
        }
        public async Task<int> SaveChanges()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task UpdateUrl(UrlModel url)
        {
            DbContext.Urls.Update(url);
            await SaveChanges();
        }

        public async Task DeleteUrl(UrlModel url)
        {
            DbContext.Urls.Remove(url);
            await SaveChanges();
        }        
    }
}
