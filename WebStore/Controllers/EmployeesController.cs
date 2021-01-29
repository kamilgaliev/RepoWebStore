using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

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

        #region Create
        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }
        #endregion

        #region Edit
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            if (id <= 0)
                return BadRequest();

            var employee = _EmployeesData.Get((int)id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateofBirth = employee.DateofBirth,
                EmploymentDate = employee.EmploymentDate
            }) ;
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if(!ModelState.IsValid) return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                DateofBirth = model.DateofBirth,
                EmploymentDate = model.EmploymentDate
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }
        #endregion
        //private Employee OneEmpl(int Id)
        //{
        //    var OneEmpl = _Employees.FirstOrDefault(e => e.Id == Id);

        //    return OneEmpl;
        //}

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateofBirth = employee.DateofBirth,
                EmploymentDate = employee.EmploymentDate
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
