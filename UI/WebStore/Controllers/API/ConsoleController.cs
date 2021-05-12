using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers.API
{
    public class ConsoleController : ControllerBase
    {
        public IActionResult Clear()
        {
            Console.Clear();
            return Ok();
        }

        public IActionResult WriteLine(string Message)
        {
            Console.WriteLine(Message);

            return Ok();
        }
    }
}
