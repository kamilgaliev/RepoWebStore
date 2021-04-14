using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
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
    }
}
