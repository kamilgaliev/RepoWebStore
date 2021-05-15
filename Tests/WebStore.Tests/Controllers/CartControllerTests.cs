﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.DTO;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public async Task CheckOut_ModelState_Invalid_Returns_View_with_Model()
        {
            const string expected_model_name = "Test order";

            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();

            var controller = new CartController(cart_service_mock.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            var order_model = new OrderViewModel { Name = expected_model_name };

            var result = await controller.CheckOut(order_model, order_service_mock.Object);

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<CartOrderViewModel>(view_result.Model);

            Assert.Equal(expected_model_name, model.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_Calls_Service_and_Returns_Redirect()
        {
            const int expected_order_id = 1;
            const string expected_user_name = "TestUser";

            var cart_service_mock = new Mock<ICartService>();
            cart_service_mock
                .Setup(s => s.GetViewModel())
                .Returns(() => new CartViewModel
                {
                    Items = new[] {(new ProductViewModel { Name = "Product"},1)}
                });

            var order_service_mock = new Mock<IOrderService>();
            order_service_mock
                .Setup(s => s.CreateOrder(It.IsAny<string>(), It.IsAny<CreateOrderModel>()))
                .ReturnsAsync(new OrderDTO
                {
                    Id = expected_order_id,
                    Name = "Order name",
                    Address = "Address",
                    Phone = "Phone",
                    Date = DateTime.Now,
                    Items = Enumerable.Empty<OrderItemDTO>(),
                });

            var controller = new CartController(cart_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] 
                        { 
                            new Claim (ClaimTypes.Name,expected_user_name)
                        }))
                    }
                }
            };

            var order_model = new OrderViewModel
            {
                Name = "Test order",
                Address = "Test address",
                Phone = "+79876543210",
            };

            var result = await controller.CheckOut(order_model, order_service_mock.Object);
            var redirect_result = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_result.ActionName);
            Assert.Null(redirect_result.ControllerName);

            Assert.Equal(expected_order_id, redirect_result.RouteValues["id"]);
        }
    }
}
