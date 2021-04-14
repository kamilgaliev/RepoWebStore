using System;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Patronymic { get; init; }

        public int Age { get; init; }

        public DateTime DateofBirth { get; init; }

        public DateTime EmploymentDate { get; init; }
    }
}
