using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; init; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(20,MinimumLength = 2,ErrorMessage = "Длина фамилии должна быть от 2 до 20 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)",ErrorMessage = "Неверный формат фамилии")]
        public string LastName { get; init; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 20 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат имени")]
        public string FirstName { get; init; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; init; }

        [Display(Name = "Возраст")]
        [Range(18,80,ErrorMessage = "Сотрудник должен быть в возрасте от 18 до 80 лет")]
        public int Age { get; init; }

        [Display(Name = "Дата рождения")]
        public DateTime DateofBirth { get; init; }

        [Display(Name = "Дата приема на работу")]
        public DateTime EmploymentDate { get; init; }
    }
}
