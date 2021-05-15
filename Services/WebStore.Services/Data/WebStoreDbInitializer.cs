using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Services.Data;

namespace WebStore.Services.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> Logger, UserManager<User> UserManager, RoleManager<Role> RoleManager)
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            _Logger.LogInformation("Инициализация базы данных .....({0:0.0###} с)", timer.Elapsed.TotalSeconds);
            //_db.Database.EnsureDeleted();
            //_db.Database.EnsureCreated();

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение миграций .....({0:0.0###} с)", timer.Elapsed.TotalSeconds);

                db.Migrate();

                _Logger.LogInformation("Выполнение миграций выполнено успешно ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
            }
            else
            {
                _Logger.LogInformation("База данных находится в актуальной версии ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
            }

            try
            {
                InitializeProducts();
                InitializeIdentityAsync().Wait();
            }
            catch (Exception error)
            {
                _Logger.LogError(error, "Ошибка при выполнении инициализации БД ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
                throw;
            }

            _Logger.LogInformation("Инициализация БД выполнено успешно ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            if(_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация  БД  товарами не требуется  ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
                return;
            }

            _Logger.LogInformation("Инициализация товаров .... ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            _Logger.LogInformation("Добавляем Sections .... ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Sections успешно добавлены ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            _Logger.LogInformation("Добавляем Brands.... ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Brands успешно добавлены  ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            _Logger.LogInformation("Добавляем Products.... ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }

            _Logger.LogInformation("Products успешно добавлены  ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            _Logger.LogInformation("Инициализация товаров выполнено успешно ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

        }

        private async Task InitializeIdentityAsync()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация системы Identity.... ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Роль отсутствует. Создаю...");
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation("Роль создана  ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
                }
            }

            await CheckRole(Role.Administrator);

            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Отсутствует учетная запись администратора");
                var admin = new User 
                {
                    UserName = User.Administrator,
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Учетная запись администратора создана");
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    _Logger.LogInformation("Учетная запись администратора наделена ролью Администратора {0}",Role.Administrator);
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании учетной записи администратора: {string.Join(",", errors)}");
                }
            }

            _Logger.LogInformation("Инициализация системы Identity завершена успешно за ({0:0.0###} с)", timer.Elapsed.TotalSeconds);

        }
    }
}
