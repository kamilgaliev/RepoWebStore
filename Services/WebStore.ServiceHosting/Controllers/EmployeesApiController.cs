using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        [HttpPost]
        public int Add(Employee employee)
        {
            return _EmployeesData.Add(employee);
        }

        [HttpPost("employee")]
        public Employee Add(string LastName, string FirstName, string Patronymic)
        {
            return _EmployeesData.Add(LastName, FirstName, Patronymic);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _EmployeesData.Delete(id);
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _EmployeesData.Get();
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _EmployeesData.Get(id);
        }

        [HttpGet("employee")]
        public Employee GetByName(string LastName, string FirstName, string Patronymic)
        {
            return _EmployeesData.GetByName(LastName, FirstName, Patronymic);
        }

        [HttpPut]
        public void Update(Employee employee)
        {
            _EmployeesData.Update(employee);
        }
    }
}
