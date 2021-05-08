using System;

namespace WebStore.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        public DateTime DateofBirth { get; set; }

        public DateTime EmploymentDate { get; set; }

        public override string ToString()
        {
            return $"{Id}: ФИО - {LastName} {FirstName} {Patronymic}, {Age} лет, Дата рождения {DateofBirth}, Прием на работу {EmploymentDate}";
        }
    }
}
