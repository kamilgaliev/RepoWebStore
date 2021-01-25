using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers
{   
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        //private List<Employee> _Employees;
        public EmployeesController(IEmployeesData EmployeesData)
        {
            //_Employees = TestData.Employees;
            _EmployeesData = EmployeesData;
        }

        //[Route("all")]
        public IActionResult Index()
        {
            //return View(_Employees);
            return View(_EmployeesData.Get());
        }

        //[Route("info(id-{id})")]
        public IActionResult Details(int id)
        {
            //if (OneEmpl(id) is not null)
            //    return View(OneEmpl(id));
            //return NotFound();
            var employee = _EmployeesData.Get(id);

            if (employee is not null)
                return View(employee);
            return NotFound();
        }

        //private Employee OneEmpl(int Id)
        //{
        //    var OneEmpl = _Employees.FirstOrDefault(e => e.Id == Id);

        //    return OneEmpl;
        //}
    }
}
