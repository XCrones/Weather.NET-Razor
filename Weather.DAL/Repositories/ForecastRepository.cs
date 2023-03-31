
using Weather.DAL.Interfaces;
using Weather.Domain.Entities;

namespace Weather.DAL.Repositories
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly AppContextDb _db;

        public ForecastRepository(AppContextDb db)
        {
            _db = db;
        }

        public async Task<bool> Create(Forecast entity)
        {
            await _db.ForecastDB.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Forecast entity)
        {
            _db.ForecastDB.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<Forecast> Read() => _db.ForecastDB;

        public async Task<Forecast> Update(Forecast entity)
        {
            _db.ForecastDB.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
