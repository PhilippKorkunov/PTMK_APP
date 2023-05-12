using MyApp;
using MyAppBusinessLayer;
using MyAppBusinessLayer.Implementations;
using MyAppDbContext;
using MyAppDbContext.Entities;
using MyAppDbLayer;
using MyAppDbLayer.Entities;
using System.Diagnostics;



Validator validator = new(args);
await validator.ValidateAsync(); // валидация данных из коносли
if (!validator.IsValid)
{
    Console.WriteLine(validator.Message);
    return;
}


DataManager dataManager = new(new EFRepository<User>(new MyAppEFDbContext()), new Repository(new MyAppEFDbContext())); //dependency injection

await using var repository = dataManager.Repository;
await using var userRepository = dataManager.EfUserRepository;

switch (validator.OperationNumber)
{
    case 1:
        string tableName = "Users";
        string createCommand = $"DROP TABLE IF EXISTS dbo.{tableName}\r\n\r\n" +
            $"CREATE TABLE dbo.{tableName}\r\n" +
            $"(\r\n\tUserId int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_User PRIMARY KEY," +
            $"\r\n\tFirstName nvarchar(50) NOT NULL," +
            $"\r\n\tSurName nvarchar(50) NOT NULL," +
            $"\r\n\tPatronymic nvarchar(50) NULL," +
            $"\r\n\tDateOfBirth date NOT NULL,\r\n\t" +
            $"Gender int  NOT NULL CHECK(Gender in (0, 1, 2))\r\n)\r\n\r\n"; //скрипт по созданию таблицы

        await repository.ExexuteNonQueryAsync(createCommand);
        Console.WriteLine("Таблица создалась");
        break;
    case 2:

        if (validator.CurrentUser is null) { Console.WriteLine("Ошибка. Данные о пользоваели не заполнены"); }
        else
        {
            await userRepository.InsertAsync(validator.CurrentUser); //добавление пользователя
            Console.WriteLine("Пользователь добавлен");
        }
        break;
    case 3:

        //Способ с помощью EF + LINQ:
        var currentUsers = await userRepository.GetAsync();

        if (currentUsers is not null)
        {
            var currentUsersList = currentUsers.DistinctBy(x => x.SurName + x.FirstName + x.Patronymic + x.DateOfBirth)
                                               .OrderBy(x => x.SurName + x.FirstName + x.Patronymic)
                                               .Take(20); // ограничем выборку для удобного отображения

            var currentUsersViewList = currentUsersList.ToUserModelViews();
            currentUsersViewList.ToConsoleViews();
        }
        else { Console.WriteLine("Query result is null"); }

        break;
    case 4:

        Generator generator = new();
        List<User> users = new();

        for (int i = 1; i <= 1000000; i++) // генерация миллиона рандомных пользователей
        {
            User user = generator.GenerateUser();
            users.Add(user);
        }

        for (int i = 1; i <= 100; i++) // генерация необходимых 100 пользователей
        {
            User user = generator.GenerateCustomUser();
            users.Add(user);
        }

        await userRepository.InsertRangeAsync(users.ToArray());
        Console.WriteLine("Рандомные пользователи добавлены");
        break;
    case 5:

        var customUsers = await userRepository.GetAsync(predicate: x => x.Gender == Gender.Male && x.SurName.StartsWith("F"));
        if (customUsers is not null)
        {
            customUsers.Take(20).ToUserModelViews().ToConsoleViews(); // вывод в консоль топ 20 записей

        }
        else { Console.WriteLine("query result is null"); }
        break;
    default:
        Console.WriteLine(validator.Message);
        break;
}