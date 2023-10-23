using WebApp.Strategy.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Strategy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host=CreateHostBuilder(args).Build();

            //üyelik-veritabanı ile ilgili işlemler bititkten sonra memoryden düşmesi için scope oluşturduk
            using var scope = host.Services.CreateScope();

            //ServiceProvider startup.cs tarafında configureServices içerisinde eklemiş olduğumuz servisleri alabilmeye imkan sağlıyor. Burada bi ctor olmadığı için ServiceProvider üzerinden alıyoruz.
            //GetRequiredService servisi varsa alır almazsa hata fırlatır. GetService alamazsa null döndürür
            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            //Migrate() komutu; uygulama ayağı kalktığında migraiton veri tabanına uygulanmadıysa bu değişiklikleri uygular. Aynı zamanda veri tabanı yok ise kendisi sıfırdan oluşturur. Varsa herhangi bir şey yapmaz.
            identityDbContext.Database.Migrate();

            if (!userManager.Users.Any())
            {
                userManager.CreateAsync(new AppUser() {UserName="user1",Email="user1@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() {UserName="user2",Email="user2@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() {UserName="user3",Email="user3@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() {UserName="user4",Email="user4@outlook.com"},"Password12*").Wait();
                userManager.CreateAsync(new AppUser() {UserName="user5",Email="user5@outlook.com"},"Password12*").Wait();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
