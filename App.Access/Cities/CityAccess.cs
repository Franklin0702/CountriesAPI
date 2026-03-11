using App.Access;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using App.Access.Entities;

namespace App.Access.Cities
{
    public class CityAccess : ICityAccess
    {
        private readonly AppDbContext _db;

        public CityAccess(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _db.Cities
                            .AsNoTracking()
                            .Include(c => c.Country)
                            .ToListAsync();
                            
        }

        public async Task<City?> GetByIdAsync(int id)
        {
            var entity = await _db.Cities.FindAsync(id);
            if (entity != null) {
                await _db.Entry(entity)
                    .Reference(c => c.Country)
                    .LoadAsync();
                return entity; 
            }

            return await _db.Cities
                            .AsNoTracking()
                            .Include(c => c.Country)
                            .FirstOrDefaultAsync(c => EF.Property<int>(c, "Id") == id);
                            
        }

        public async Task<City> AddAsync(City City)
        {
            if (City == null) throw new ArgumentNullException(nameof(City));

            _db.Cities.Add(City);
            await _db.SaveChangesAsync();
            await _db.Entry(City)
                .Reference(c => c.Country)
                .LoadAsync();
            return City;
        }

        public async Task<City> UpdateAsync(City City)
        {
            if (City == null) throw new ArgumentNullException(nameof(City));

            // Attach if not tracked
            var entry = _db.Entry(City);
            if (entry.State == EntityState.Detached)
            {
                _db.Cities.Attach(City);
                entry = _db.Entry(City);
            }

            entry.State = EntityState.Modified;
            await _db.SaveChangesAsync();
            await entry
                .Reference(c => c.Country)
                .LoadAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Cities.FindAsync(id);
            if (entity == null) return;

            _db.Cities.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Cities.AnyAsync(c => EF.Property<int>(c, "Id") == id);
        }
    }
}
