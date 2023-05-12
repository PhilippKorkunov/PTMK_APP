using MyAppDbContext;
using MyAppDbContext.Entities;

namespace MyApp
{
    public class Generator
    {
        public User GenerateUser()
        { 
            string surname = GenerateWord();
            string firstName = GenerateWord();
            string patronymic = GenerateWord();
            DateTime dateOfBirth = GenerateDateTime();
            Gender gender = GenerateGender();

            return new User(firstName, surname, patronymic, dateOfBirth, gender);
        }

        public User GenerateCustomUser() // генератор пользователя
        {
            string surname = GenerateWord('F');
            string firstName = GenerateWord();
            string patronymic = GenerateWord();
            DateTime dateOfBirth = GenerateDateTime();
            Gender gender = Gender.Male;

            return new User(firstName, surname, patronymic, dateOfBirth, gender);
        }

        
        private static string GenerateWord(char firstLetter = ' ') //генератор слов
        {
            string upperletterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowerLetterSet = upperletterSet.ToLower();

            Random random = new Random();
            int wordLength = random.Next(3, 7); //ограничение на длину слова
            string word = string.Empty;

            for (int i = 0; i < wordLength; i++)
            {
                if (i == 0) 
                {
                    word +=  firstLetter == ' ' ?  upperletterSet[random.Next(0, upperletterSet.Length - 1)] : firstLetter.ToString().ToUpper(); //первая буква заглавная
                }
                else
                {
                    word += lowerLetterSet[random.Next(0, lowerLetterSet.Length - 1)];
                }
            }
            return word;
        }

        private DateTime GenerateDateTime()
        {
            Random random = new Random();
            int randomDays = random.Next(2000, 12000);
            return DateTime.Today.AddDays(-randomDays); //вычитание из текущей даты от 2000 до 12000 дней
        }



        private Gender GenerateGender() => (Gender)new Random().Next(0, 2); //генератор пола
    }
}
