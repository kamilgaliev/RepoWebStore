using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        private readonly ILogger<EmployeesClient> _Logger;

        public EmployeesClient(IConfiguration Configuration,ILogger<EmployeesClient> Logger) : base(Configuration, WebAPI.Employees)
        {
            _Logger = Logger;
        }

        public int Add(Employee employee) => Post(Address, employee).Content.ReadAsAsync<int>().Result;

        public Employee Add(string LastName, string FirstName, string Patronymic, int Age) => 
            Post($"{Address}/employee?LastName={LastName}&FirstName={FirstName}&Patronymic={Patronymic}&Age={Age}","").
            Content.ReadAsAsync<Employee>().Result;

        public bool Delete(int id)
        {
            _Logger.LogInformation($"Удаление сотрудника с id = {id}...");

            using (_Logger.BeginScope($"Удаление сотрудника с id = {id}"))
            {
                var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
                _Logger.LogInformation("Удаление сотрудника с id = {0} - {1}", id, result ? "выполнено" : "не найден!");
                return result;
            }
            
        }

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(Address);

        public Employee Get(int id) => Get<Employee>($"{Address}/{id}");

        public Employee GetByName(string LastName, string FirstName, string Patronymic) =>
            Get<Employee>($"{Address}/employee?LastName={LastName}&FirstName={FirstName}&Patronymic={Patronymic}");

        public void Update(Employee employee) => Put(Address, employee);
    }
}
