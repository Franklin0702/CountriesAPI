using App.Access;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using App.Access.Entities;

namespace App.Access.Countries
{
    public class CountryAccess(AppDbContext db) : ICountryAccess
    {
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await db.Countries
                .AsNoTracking()
                .Include(c => c.Cities)
                .ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            var entity = await db.Countries.FindAsync(id);
            if (entity != null)
            {
                await db.Entry(entity)
                    .Collection(c => c.Cities)
                    .LoadAsync();
            }

            return await db.Countries
                .AsNoTracking()
                .Include(c => c.Cities)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country> AddAsync(Country country)
        {
            ArgumentNullException.ThrowIfNull(country, nameof(country));

            db.Countries.Add(country);
            await db.SaveChangesAsync();
            return country;
        }

        public async Task<Country> UpdateAsync(Country country)
        {
            ArgumentNullException.ThrowIfNull(country, nameof(country));

            // Attach if not tracked
            var entry = db.Entry(country);
            if (entry.State == EntityState.Detached)
            {
                db.Countries.Attach(country);
                entry = db.Entry(country);
            }
            
            // Patching because coordinates can't be modified after creation.
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                if (property.CurrentValue != null)
                {
                    property.IsModified = true;
                }
            }
            await db.SaveChangesAsync();
            await entry
                .Collection(c => c.Cities)
                .LoadAsync();
            return entry.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await db.Countries.FindAsync(id);
            if (entity == null) return;

            db.Countries.Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await db.Countries.AnyAsync(c => EF.Property<int>(c, "Id") == id);
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            return !db.Countries.AsNoTracking().Any(c => c.CountryCode == code);
        }
    }
}
