using MyAppDbContext;
using MyAppDbContext.Entities;

namespace MyApp.Model
{
    public class UserModelView : IViewModel
    {
        public int UserId { get; private set; }

        public string SurName { get; private set; }

        public string FirstName { get; private set; }

        public string Patronymic { get; private set; }

        public string DateOfBirth { get; private set; }
        public int Age { 
            get 
            {
                DateTime today = DateTime.Today;
                DateTime dateOfBirth = DateTime.Parse(DateOfBirth);

                int age = today.Year - dateOfBirth.Year;
                if (dateOfBirth.AddYears(age) > today)
                {
                    age--;
                }
                return age;
            } 
            private set { } }

        public Gender Gender { get; private set; }

        public UserModelView(User users)
        {
            UserId = users.UserId;
            FirstName = users.FirstName;
            SurName = users.SurName;
            Patronymic = users.Patronymic is null ? string.Empty : users.Patronymic;
            DateOfBirth = users.DateOfBirth.ToString("dd.MM.yyyy");
            Gender = users.Gender;
        }
    }
}
