using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{   
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private List<Employee> _Employees;
        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        //[Route("all")]
        public IActionResult Index()
        {
            return View(_Employees);
        }

        //[Route("info(id-{id})")]
        public IActionResult Details(int id)
        {
            if (OneEmpl(id) is not null)
                return View(OneEmpl(id));
            return NotFound();
        }

        private Employee OneEmpl(int Id)
        {
            var OneEmpl = _Employees.FirstOrDefault(e => e.Id == Id);

            return OneEmpl;
        }
    }
}
