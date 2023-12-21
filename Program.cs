using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Пространство имен для основной части кода приложения телефонной книги
namespace phone_book_app
{
    // Простой логгер для записи сообщений различных уровней
    public class Logger
    {
        // Статический метод для логирования сообщений
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine($"{DateTime.Now} [{level}] {message}");
        }
    }

    // Перечисление для уровней логирования
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }

    // Класс, представляющий контакт в телефонной книге
    public class Contact
    {
        // Свойства для хранения информации о контакте
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Конструктор для создания нового контакта
        public Contact(string firstName, string lastName, string phoneNumber, string email, string address = "")
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }

    // Класс для представления телефонной книги
    public class MyPhoneBook
    {
        private readonly List<Contact> contacts;  // Список контактов

        // Конструктор, принимающий список контактов при создании телефонной книги
        public MyPhoneBook(List<Contact> contacts)
        {
            this.contacts = contacts;
        }

        // Метод для добавления нового контакта
        public void AddContact(Contact contact)
        {
            // Проверка наличия контакта с таким же номером телефона
            if (contacts.Any(c => c.PhoneNumber == contact.PhoneNumber))
            {
                Logger.Log("Контакт успешно удален.", LogLevel.Info);
                return;
            }

            contacts.Add(contact);
            Logger.Log("Контакт успешно добавлен.", LogLevel.Info);
        }

        // Метод для удаления контакта по имени, фамилии или номеру телефона
        public void RemoveContact(string searchTerm)
        {
            var contactToRemove = contacts.FirstOrDefault(c =>
                c.FirstName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.LastName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.PhoneNumber.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (contactToRemove != null)
            {
                contacts.Remove(contactToRemove);
                Logger.Log("Контакт успешно удален.", LogLevel.Info);
            }
            else
            {
                Logger.Log("Контакт не найден.", LogLevel.Warning);
            }
        }

        // Метод для редактирования контакта
        public void EditContact(string searchTerm, Contact updatedContact)
        {
            var contactToEdit = contacts.FirstOrDefault(c =>
                c.FirstName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.LastName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.PhoneNumber.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (contactToEdit != null)
            {
                contacts.Remove(contactToEdit);
                contacts.Add(updatedContact);
                Console.WriteLine("Контакт успешно отредактирован.");
            }
            else
            {
                Console.WriteLine("Контакт для редактирования не найден.");
            }
        }

        // Метод для поиска контакта по имени, фамилии или номеру телефона
        public void FindContact(string searchTerm)
        {
            var foundContacts = contacts.Where(c =>
                c.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.PhoneNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (foundContacts.Any())
            {
                Console.WriteLine("Найденные контакты:");
                foreach (var contact in foundContacts)
                {
                    Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.PhoneNumber}");
                }
            }
            else
            {
                Console.WriteLine("Контакт не найден.");
            }
        }

        // Метод для вывода всех контактов, отсортированных по фамилии и имени
        public void ShowAllContacts()
        {
            var sortedContacts = contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);

            if (sortedContacts.Any())
            {
                Console.WriteLine("Все контакты:");
                foreach (var contact in sortedContacts)
                {
                    Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.PhoneNumber}");
                }
            }
            else
            {
                Console.WriteLine("Список контактов пуст.");
            }
        }
    }

    // Основной класс программы
    internal class Program
    {
        private static readonly MyPhoneBook phoneBook = new MyPhoneBook(new List<Contact>());

        // Главный метод программы
        static void Main(string[] args)
        {
            // Бесконечный цикл для отображения меню и выполнения действий
            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Добавить контакт");
                Console.WriteLine("2. Удалить контакт");
                Console.WriteLine("3. Редактировать контакт");
                Console.WriteLine("4. Поиск контакта");
                Console.WriteLine("5. Вывести все контакты");
                Console.WriteLine("6. Выход");
                Console.WriteLine("Выберите действие (введите цифру):");

                // Попытка преобразовать ввод пользователя в целое число
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    // Обработка выбора пользователя
                    switch (choice)
                    {
                        case 1:
                            AddContact();
                            break;
                        case 2:
                            RemoveContact();
                            break;
                        case 3:
                            EditContact();
                            break;
                        case 4:
                            FindContact();
                            break;
                        case 5:
                            ShowAllContacts();
                            break;
                        case 6:
                            Environment.Exit(0);
                            break;
                        default:
                            Logger.Log("Неверный ввод. Пожалуйста, выберите действие от 1 до 6.", LogLevel.Warning);
                            break;
                    }
                }
                else
                {
                    Logger.Log("Неверный ввод. Пожалуйста, введите число.", LogLevel.Warning);
                }
            }
        }

        // Метод для добавления нового контакта
        private static void AddContact()
        {
            Console.WriteLine("Введите имя:");
            var firstName = Console.ReadLine();

            Console.WriteLine("Введите фамилию:");
            var lastName = Console.ReadLine();

            Console.WriteLine("Введите номер телефона:");
            var phoneNumber = Console.ReadLine();

            Console.WriteLine("Введите адрес электронной почты:");
            var email = Console.ReadLine();

            Console.WriteLine("Введите адрес (необязательно):");
            var address = Console.ReadLine();

            var newContact = new Contact(firstName, lastName, phoneNumber, email, address);
            phoneBook.AddContact(newContact);
        }

        // Метод для удаления контакта
        private static void RemoveContact()
        {
            Console.WriteLine("Введите имя, фамилию или номер телефона контакта для удаления:");
            var searchTerm = Console.ReadLine();

            phoneBook.RemoveContact(searchTerm);
        }

        // Метод для редактирования контакта
        private static void EditContact()
        {
            Console.WriteLine("Введите имя, фамилию или номер телефона контакта для редактирования:");
            var searchTerm = Console.ReadLine();

            Console.WriteLine("Введите новое имя:");
            var newFirstName = Console.ReadLine();

            Console.WriteLine("Введите новую фамилию:");
            var newLastName = Console.ReadLine();

            Console.WriteLine("Введите новый номер телефона:");
            var newPhoneNumber = Console.ReadLine();

            Console.WriteLine("Введите новый адрес электронной почты:");
            var newEmail = Console.ReadLine();

            Console.WriteLine("Введите новый адрес (необязательно):");
            var newAddress = Console.ReadLine();

            var updatedContact = new Contact(newFirstName, newLastName, newPhoneNumber, newEmail, newAddress);
            phoneBook.EditContact(searchTerm, updatedContact);
        }

        // Метод для поиска контакта
        private static void FindContact()
        {
            Console.WriteLine("Введите имя, фамилию, номер телефона для поиска:");
            var searchTerm = Console.ReadLine();

            phoneBook.FindContact(searchTerm);
        }

        // Метод для вывода всех контактов
        private static void ShowAllContacts()
        {
            phoneBook.ShowAllContacts();
        }
    }
}
