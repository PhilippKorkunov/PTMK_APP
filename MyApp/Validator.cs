using MyAppDbContext;
using MyAppDbContext.Entities;
using System.Text.RegularExpressions;

namespace MyApp
{
    public class Validator
    {
        //Регулярные выражения для валидации:
        private readonly string[] args;
        private Regex? regex;
        private static readonly string operationPattern = @"^[1-5]$";
        private static readonly string namePattern = @"[A-Z]{1}[a-z]{1,}";
        private static readonly string datePattern = @"\d\d(\.|-|\\)\d\d(\.|-|\\)\d\d\d\d";
        private static readonly string genderPattern = @"(M|F|U)";


        public User? CurrentUser { get; private set; }
        public int OperationNumber { get; private set; }
        public bool IsValid { get; private set; }
        public string Message { get; private set; } // сообщение с описанием про валидацию

        public Validator(string[] args)
        {
            this.args = args;
            Message = "Валидация еще не была пройдена";
        }

        public void Validate()
        {
            if (args.Length == 0) {Message = "Номер операции не был введен"; return; }
            regex = new(operationPattern);
            if (!regex.IsMatch(args[0])) { Message = "Номер операции был введен неверно"; return; }
            OperationNumber = Int32.Parse(args[0]);
            if (OperationNumber != 2 && args.Length > 1)
            {
                Message = "Вводная строка содержит лишние парметры после номера";
                return;
            }
            else if (OperationNumber == 2)
            {
                if (args.Length == 6 || args.Length == 5)
                {
                    regex = new Regex(namePattern);
                    if (!regex.IsMatch(args[1])) { Message = "Фамилия была введена неверно. Ожидаемый формат: Фамилия - слово на латинице с заглавной буквы"; return; }
                    if (!regex.IsMatch(args[2])) { Message = "Имя было введена неверно. Ожидаемый формат: Имя - слово на латинице с заглавной буквы"; return; }
                    string surname = args[1];
                    string name = args[2];
                    int i = 0;
                    string? patronimic;
                    if (args.Length == 5)
                    {
                        i = 1;
                        patronimic = null;

                    }
                    else
                    {
                        if (!regex.IsMatch(args[3])) { Message = "Отчество было введена неверно. Ожидаемый формат: Отчество - слово на латинице с заглавной буквы"; return; }
                        patronimic = args[3];
                    }
                    regex = new Regex(datePattern);
                    if (!regex.IsMatch(args[4-i])) { Message = "Неверный формат даты. Ожидаемый формат:  dd.mm.yyyy. В качестве делимтра можнт использоваться и '\\', и '-'"; return; }
                    DateTime date = DateTime.Parse(args[4-i]);
                    regex = new Regex(genderPattern);
                    if (!regex.IsMatch(args[5 - i])) { Message = "Неверный формат пола. Ожидаемый один из символов: M (мужской пол), F(женский пол), U(неопределено)"; return; }
                    Gender gender = args[5-i] ==  "M" ? Gender.Male : args[5-1] == "F" ? Gender.Female : Gender.Undefined;

                    CurrentUser = new User(name, surname, patronimic, date, gender);
                }
                else
                {
                    Message = "Вводная строка имеет неверный формат.\nОжидаемый формат: <Номер операции> <Фамилия> <Имя> <Отчество> <Дата рождения> <Пол> или " +
                        "<Номер операции> <Фамилия> <Имя> <Дата рождения> <Пол>\nНомер операции - число от 1 до 5 включительно\nФамилия, имя и отчество - слова на латинице с заглавной буквы." +
                        "Дата в формате dd.mm.yyyy. В качестве делимтра можнт использоваться и '\\', и '-'\nПол - M (мужской пол), F(женский пол), U(неопределено)";
                    return;
                }
            }

            Message = "Валидация пройдена";
            IsValid = true;
        }

        public async Task ValidateAsync() => await Task.Run(() => Validate());
    }
}
