using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Memory {

    class Student {
        [Key]
        public string Id { set; get; }
        public string Name { set; get; } = Guid.NewGuid().ToString("N");
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { set; get; }
    }

    class Program {

        static async Task<int> InsertAsync(DbContextOptions options) {
            using var context = new MyContext(options);
            context.Database.EnsureCreated();
            context.Students.AddRange(new[] {
                new Student(),
                new Student(),
                new Student(),
             });
            return await context.SaveChangesAsync();
        }

        static async Task<List<Student>> QueryAsync(DbContextOptions options) {
            using var context = new MyContext(options);
            return await context.Students.ToListAsync();
        }

        static async Task Main(string[] args) {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("InMemory").Options;

            var count = await InsertAsync(options);
            var data = await QueryAsync(options);

            Console.WriteLine(count == data.Count);
        }
    }
}
