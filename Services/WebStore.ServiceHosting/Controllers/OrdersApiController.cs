using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>
    /// Управление заказами
    /// </summary>
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase,IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        /// <summary>
        /// Создание нового заказа
        /// </summary>
        /// <param name="UserName">Имя пользователя</param>
        /// <param name="OrderModel">Информация о заказе</param>
        /// <returns>Информация о сформированном заказе</returns>
        [HttpPost("{UserName}")]
        public async Task<OrderDTO> CreateOrder(string UserName,[FromBody] CreateOrderModel OrderModel)
        {
            return await _OrderService.CreateOrder(UserName, OrderModel);
        }

        /// <summary>
        /// Получение заказа по его ид
        /// </summary>
        /// <param name="id">ИД заказа</param>
        /// <returns>Информация о заказе</returns>
        [HttpGet("{id:int}")]
        public async Task<OrderDTO> GetOrderById(int id)
        {
            return await _OrderService.GetOrderById(id);
        }

        /// <summary>
        /// Получение всех заказов пользователя
        /// </summary>
        /// <param name="UserName">Имя пользователя</param>
        /// <returns>Заказы пользователя</returns>
        [HttpGet("user/{UserName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName)
        {
            return await _OrderService.GetUserOrders(UserName);
        }
    }
}
