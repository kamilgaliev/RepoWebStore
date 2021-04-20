using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesApiController> _Logger;

        public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        [HttpPost]
        public int Add(Employee employee)
        {
            _Logger.LogInformation($"Добавление сотрудника {employee}");
            return _EmployeesData.Add(employee);
        }

        [HttpPost("employee")]
        public Employee Add(string LastName, string FirstName, string Patronymic, int Age)
        {
            _Logger.LogInformation($"ФИО - {LastName} {FirstName} {Patronymic}, {Age} лет");
            return _EmployeesData.Add(LastName, FirstName, Patronymic,Age);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            _Logger.LogInformation($"Удаление сотрудника с id = {id}");
            var result = _EmployeesData.Delete(id);
            _Logger.LogInformation("Удаление сотрудника с id = {0} - {1}",id, result ? "выполнено" : "не найден!");
            return result;
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
            _Logger.LogInformation($"Редактирование сотрудника {employee}");
            _EmployeesData.Update(employee);
        }
    }
}
