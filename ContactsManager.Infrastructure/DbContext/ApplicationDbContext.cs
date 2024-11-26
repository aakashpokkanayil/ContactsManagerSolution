using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace PersonData.PersonsContext
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Person>? Persons { get; set; }
        public virtual DbSet<Country>? Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // we can define table name like this here "Countries",
            // so we can define our own name for tables other than
            // this property name public DbSet<Person> Persons { get; set; }
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seeding Data
            string countries= 
                File.ReadAllText("C:\\Users\\aakas\\OneDrive\\Web Development\\ContactsManagerSolution\\" +
                "ContactsManager.Infrastructure\\SeedData\\Countries.json");
            List<Country>? countriesList= JsonSerializer.Deserialize<List<Country>>(countries);
            if(countriesList!=null) modelBuilder.Entity<Country>().HasData(countriesList);

            string persons = File.ReadAllText("C:\\Users\\aakas\\OneDrive\\Web Development\\ContactsManagerSolution\\" +
                "ContactsManager.Infrastructure\\SeedData\\Persons.json");
            List<Person>? personsList = JsonSerializer.Deserialize<List<Person>>(persons);
            if (personsList != null) modelBuilder.Entity<Person>().HasData(personsList);

        }
    }
}
