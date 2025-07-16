using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayTracker.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Контактные данные")]
        public string Address { get; set; }

        [Display(Name = "Фотография")]
        public string PhotoPath { get; set; }

        public string GetBirthdayStatus()
        {
            var today = DateTime.Today;
            if (BirthDate.Month < today.Month || (BirthDate.Month == today.Month && BirthDate.Day < today.Day))
            {
                return "Прошло";
            }
            if (BirthDate.Month == today.Month && BirthDate.Day == today.Day)
            {
                return "Сегодня";
            }
            return "Будет";
        }

        public int CalculateAge()
        {
            var today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}