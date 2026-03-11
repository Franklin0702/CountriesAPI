using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using App.Access;
using App.Access.Cities;
using App.Access.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace App.Tests
{
    public class CityAccessTests
    {
        private DbContextOptions<AppDbContext> CreateOptions() =>
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;

        [Test]
        public async Task Add_Get_Update_Delete_Exists_Workflow()
        {
            var options = CreateOptions();

            // seed country
            using (var db = new AppDbContext(options))
            {
                db.Countries.Add(new Country { CountryName = "TestCountry", CountryCode = "TC" });
                await db.SaveChangesAsync();
            }

            using (var db = new AppDbContext(options))
            {
                var access = new CityAccess(db);

                var country = await db.Countries.FirstAsync();

                var added = await access.AddAsync(new City { Name = "CityA", CountryId = country.Id });
                Assert.That(added.Id, Is.GreaterThan(0));
                Assert.That(added.Country, Is.Not.Null);

                var fetched = await access.GetByIdAsync(added.Id);
                Assert.That(fetched, Is.Not.Null);
                Assert.That(fetched!.Name, Is.EqualTo("CityA"));
                Assert.That(fetched.Country, Is.Not.Null);

                var all = (await access.GetAllAsync()).ToList();
                Assert.That(all, Has.Exactly(1).Items);

                added.Name = "CityB";
                var updated = await access.UpdateAsync(added);
                Assert.That(updated.Name, Is.EqualTo("CityB"));

                var exists = await access.ExistsAsync(updated.Id);
                Assert.That(exists, Is.True);

                await access.DeleteAsync(updated.Id);
                var existsAfterDelete = await access.ExistsAsync(updated.Id);
                Assert.That(existsAfterDelete, Is.False);
            }
        }
    }
}
