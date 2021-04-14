using System;
using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new()
        {
            new Employee
            {
                Id = 1,
                LastName = "Иванов",
                FirstName = "Иван",
                Patronymic = "Иванович",
                Age = 20,
                DateofBirth = new DateTime(2001, 7, 20),
                EmploymentDate = new DateTime(2020, 01, 10)
            },
            new Employee
            {
                Id = 2,
                LastName = "Сергеев",
                FirstName = "Сергей",
                Patronymic = "Иванович",
                Age = 30,
                DateofBirth = new DateTime(1991, 5, 10),
                EmploymentDate = new DateTime(2015, 05, 10)
            },
            new Employee
            {
                Id = 3,
                LastName = "Колобков",
                FirstName = "Колобок",
                Patronymic = "Владимирович",
                Age = 30,
                DateofBirth = new DateTime(1991, 9, 20),
                EmploymentDate = new DateTime(2016, 03, 11)
            }
        };
    }
}
