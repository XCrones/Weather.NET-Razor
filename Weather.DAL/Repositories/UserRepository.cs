using Weather.DAL.Interfaces;
using Weather.Domain.Entities;

namespace Weather.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppContextDb _db;

        public UserRepository(AppContextDb db)
        {
            _db = db;
        }

        public async Task<bool> Create(User entity)
        {
            await _db.UserDB.AddAsync(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(User entity)
        {
            _db.UserDB.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public IQueryable<User> Read() => _db.UserDB;

        public async Task<User> Update(User entity)
        {
            _db.UserDB.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
