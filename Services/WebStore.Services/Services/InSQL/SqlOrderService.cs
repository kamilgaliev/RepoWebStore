using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {UserName} не найден в БД");

            await using var transaction = await _db.Database.BeginTransactionAsync().ConfigureAwait(false);

            var order = new Order 
            {
                Name = OrderModel.Name,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                User = user,
            };

            var products_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

            var cart_products = await _db.Products
                .Where(p => products_ids.Contains(p.Id))
                .ToArrayAsync();

            order.Items = Cart.Items.Join
                (
                    cart_products,
                    cart_item => cart_item.Product.Id,
                    product => product.Id,
                    (cart_item, product) => new OrderItem
                    {
                        Order = order,
                        Product = product,
                        Price = product.Price, //Для скидок можно здесь
                        Quantity = cart_item.Quantity
                    }).ToArray();

            await _db.Orders.AddAsync(order);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }

        public async Task<Order> GetOrderById(int id) => await _db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id);

        public async Task<IEnumerable<Order>> GetUserOrders(string UserName) => await _db.Orders
            .Include(order => order.User)
            .Include(order => order.Items)
            .Where(order => order.User.UserName == UserName)
            .ToArrayAsync();
    }
}
