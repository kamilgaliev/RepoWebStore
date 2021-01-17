using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age=20, DateofBirth = new DateTime(2001,7,20), 
                EmploymentDate = new DateTime(2020,01,10) },
            new Employee { Id = 2, LastName = "Сергеев", FirstName = "Сергей", Patronymic = "Иванович", Age = 30, DateofBirth = new DateTime(1991, 5, 10),
                EmploymentDate = new DateTime(2015, 05, 10)},
            new Employee { Id = 3, LastName = "Колобков", FirstName = "Колобок", Patronymic = "Владимирович", Age = 30, DateofBirth = new DateTime(1991, 9, 20),
                EmploymentDate = new DateTime(2016, 03, 11)}
        };
        public IActionResult Index() => View();

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Employees()
        {
            return View(__Employees);
        }

        public IActionResult Details(int id)
        {
            return View(OneEmpl(id));
        }

        private Employee OneEmpl(int Id)
        {
            Employee OneEmpl = new Employee();
            foreach (var empl in __Employees)
            {
                if (Id == empl.Id)
                    OneEmpl = empl;
            }
            return OneEmpl;
        }
    }
}
