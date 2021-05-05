using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Domain.DTO;
using System.Linq;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService CartService)
        {
            _CartService = CartService;
        }
        public IActionResult Index()
        {
            return View(new CartOrderViewModel
            {
                Cart = _CartService.GetViewModel()
            });
        }

        public IActionResult Add(int id)
        {
            _CartService.Add(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            _CartService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int id)
        {
            _CartService.Decrement(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            _CartService.Clear();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel,[FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel 
                {
                    Cart = _CartService.GetViewModel(),
                    Order = OrderModel
                });

            //var order = await OrderService.CreateOrder
            //    (
            //        User.Identity!.Name,
            //        _CartService.GetViewModel(),
            //        OrderModel
            //    );

            var order_model = new CreateOrderModel
            {
                Order = OrderModel,
                Items = _CartService.GetViewModel().Items.Select(item => new OrderItemDTO 
                {
                    Id = item.Product.Id,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                }).ToList(),
            };

            var order = await OrderService.CreateOrder(User.Identity!.Name,order_model);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region WebAPI

        public IActionResult GetCartView()
        {
            return ViewComponent("Cart");
        }

        public IActionResult AddAPI(int id)
        {
            _CartService.Add(id);
            return Ok();
        }

        public IActionResult RemoveAPI(int id)
        {
            _CartService.Remove(id);
            return Ok();
        }

        public IActionResult DecrementAPI(int id)
        {
            _CartService.Decrement(id);
            return Ok();
        }

        public IActionResult ClearAPI()
        {
            _CartService.Clear();
            return Ok();
        }

        #endregion
    }
}
