using System;
using System.Collections.Generic;
using System.Text.Json;

class Person
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string address { get; set; }

    public override string ToString()
    {
        return $"{Name} — {BirthDate:dd.MM.yyyy} - {address}";
    }
    public string CheckStatement()
    {
        var today = DateTime.Now;
        if (BirthDate.Month < today.Month || (BirthDate.Month == today.Month && BirthDate.Day < today.Day))
        {
            return "Прошло";
        }
        if (BirthDate.Month == today.Month && BirthDate.Day == today.Day)
        {
            return "Сегодня";
        }
        if (BirthDate.Month > today.Month || (BirthDate.Month == today.Month && BirthDate.Day > today.Day))
        {
            return "Будет";
        }
        else
        {
            return "Ошибка состояния";
        }
    }
    public bool IsBirthdayToday()
    {
        var today = DateTime.Today;
        return BirthDate.Day == today.Day && BirthDate.Month == today.Month;
    }
}

class Program
{
    static readonly string FilePath = "people.json";

    static void SaveToFile(List<Person> people)
    {
        var json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
        Console.WriteLine(" Список сохранён в файл.");
    }

    static List<Person> LoadFromFile()
    {
        if (!File.Exists(FilePath)) return new List<Person>();
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
    }
    static Person CreatePerson()
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите дату рождения (ГГГГ-ММ-ДД): ");
        DateTime birthDate;

        while (!DateTime.TryParse(Console.ReadLine(), out birthDate))
        {
            Console.WriteLine("Некорректный формат. Попробуйте ещё раз:");
        }
        Console.WriteLine("Введите контактные данные пользователя : email/номер телефона(при наличии)");
        string place = Console.ReadLine();
        return new Person { Name = name, BirthDate = birthDate, address = place };
    }
    static void DeletePerson(List<Person> people)
    {
        if (people.Count == 0)
        {
            Console.WriteLine("Список пуст, операция отменена");
            return;
        }
        ListOutput(people);
        Console.WriteLine("Выберите номер записи, которую хотите удалить");
        string choise = Console.ReadLine();
        int c = Convert.ToInt32(choise);
        if (c <= 0 || c > people.Count)
        {
            Console.WriteLine("Такой записи нет,операция отменена");
            return;
        }
        else
        {
            people.RemoveAt(c - 1);
            Console.WriteLine("Запись успешно удалена");
        }
    }
    static void ListOutput(List<Person> people)
    {
        if (people.Count == 0)
        {
            Console.WriteLine("В списке пока нет людей");
            return;
        }
        Console.WriteLine("Дни рождения : ");
        int count = 1;
        foreach (var p in people)
        {
            Console.WriteLine($"{count}. {p}");
            count++;
        }
    }
    static void ChangePeople(List<Person> people)
    {
        if (people.Count == 0)
        {
            Console.WriteLine("В списке нет людей,операция отменена.");
            return;
        }
        ListOutput(people);
        Console.WriteLine("Выберите номер записи, которую хотите изменить");
        string choise = Console.ReadLine();
        int c = Convert.ToInt32(choise);
        if (c <= 0 || c > people.Count)
        {
            Console.WriteLine("Такого человека нет в списке, операция отменена");
        }
        else
        {
            bool check = true;
            while (check)
            {
                Console.WriteLine("Текущая информация о пользователе : \n" + people[c - 1]);
                Console.WriteLine("Выберите операцию : \n1.Изменить имя\n2.Изменить дату рождения\n3.Изменить контактные данные\n4.Закончить изменение");
                string l = Console.ReadLine();
                switch (l)
                {
                    case "1":
                        Console.WriteLine("Введите имя пользователя :");
                        string name = Console.ReadLine();
                        people[c - 1].Name = name;
                        break;
                    case "2":
                        Console.WriteLine("Введите дату рождения пользователя(ГГГГ-ММ-ДД) :");
                        DateTime birthDate;

                        while (!DateTime.TryParse(Console.ReadLine(), out birthDate))
                        {
                            Console.WriteLine("Некорректный формат. Попробуйте ещё раз:");
                        }
                        people[c - 1].BirthDate = birthDate;
                        break;
                    case "3":
                        Console.WriteLine("Введите контактные данные пользователя : email/Номер телефона");
                        string place = Console.ReadLine();
                        people[c - 1].address = place;
                        break;
                    case "4":
                        check = false;
                        break;
                    default:
                        Console.WriteLine("Такой команды нет, попробуйте снова");
                        break;
                }
            }
        }
    }
    static void CurrentBirthDays(List<Person> people)
    {
        if (people.Count == 0)
        {
            Console.WriteLine("Список пуст");
            return;
        }
        int count = 0;
        Console.WriteLine("Дни рождения в этом месяце, которые ещё актуальны :");
        foreach (var p in people)
        {
            if (p.IsBirthdayToday() == true)
            {
                Console.WriteLine($"У {p.Name} сегодня день рождение! Ему исполняется {2025 - p.BirthDate.Year}!");
                count++;
            }
            if (p.BirthDate.Month == DateTime.Now.Month && p.BirthDate.Day > DateTime.Now.Day)
            {
                count++;
                Console.WriteLine(p);
            }
        }
        if (count == 0)
        {
            Console.WriteLine("В этом месяце нет актуальных дней рождений.");
        }
    }
    static void SortByMonthAndDay(List<Person> people)
    {
        people.Sort((a, b) =>
        {
            int result = a.BirthDate.Month.CompareTo(b.BirthDate.Month);
            if (result == 0)
            {
                result = a.BirthDate.Day.CompareTo(b.BirthDate.Day);
            }
            return result;
        });
    }
    static void CuteOutput(List<Person> people)
    {
        if (people.Count == 0)
        {
            Console.WriteLine(" В списке нет записей.");
            return;
        }

        const int pageSize = 5;
        int totalPages = (int)Math.Ceiling(people.Count / (double)pageSize);
        int currentPage = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($" Страница {currentPage} из {totalPages}");
            Console.WriteLine(new string('-', 65));
            Console.WriteLine($"{"Имя",-15} | {"Дата рождения",-12} | {"Контакты",-20} | Статус |");
            Console.WriteLine(new string('-', 65));

            int start = (currentPage - 1) * pageSize;
            int end = Math.Min(start + pageSize, people.Count);

            for (int i = start; i < end; i++)
            {
                var p = people[i];
                Console.WriteLine($"{p.Name,-15} | {p.BirthDate:dd.MM.yyyy} | {p.address,-23} | {p.CheckStatement()} |");
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine("n — следующая | p — предыдущая | q — выход");
            Console.Write("Выбор: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "n" && currentPage < totalPages)
                currentPage++;
            else if (input == "p" && currentPage > 1)
                currentPage--;
            else if (input == "q")
                break;
        }
    }

    static void Main()
    {
        List<Person> people = LoadFromFile();
        CurrentBirthDays(people);
        bool programm = true;
        Console.WriteLine("\n");
        while (programm)
        {
            Console.WriteLine("//--------Меню программы--------// \n1.Добавление записи в список\n2.Вывод списка\n3.Изменить информацию о записи\n4.Удаление записи из списка\n5.Ближайшие дни рождения\n6.Сортировка списка\n0.Выход из программы");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    SaveToFile(people);
                    programm = false;
                    break;
                case "1":
                    var person = CreatePerson();
                    people.Add(person);
                    break;
                case "2":
                    CuteOutput(people);
                    break;
                case "3":
                    ChangePeople(people);
                    break;
                case "4":
                    DeletePerson(people);
                    break;
                case "5":
                    CurrentBirthDays(people);
                    break;
                case "6":
                    SortByMonthAndDay(people);
                    break;
                default:
                    Console.WriteLine("Такого варианта нет, попробуйте снова!");
                    break;
            }

        }
    }
}
