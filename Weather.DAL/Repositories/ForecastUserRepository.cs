using Weather.DAL.Interfaces;
using Weather.Domain.Entities;

namespace Weather.DAL.Repositories
{
    public class ForecastUserRepository : IForecastUserRepository
    {
        private readonly AppContextDb _db;

        public ForecastUserRepository(AppContextDb db)
        {
            _db = db;
        }

        public async Task<bool> Create(ForecastUser entity)
        {
            await _db.ForecastUserDB.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(ForecastUser entity)
        {
            _db.ForecastUserDB.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<ForecastUser> Read() => _db.ForecastUserDB;

        public async Task<ForecastUser> Update(ForecastUser entity)
        {
            _db.ForecastUserDB.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
