using MyAppDbLayer.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAppDbContext.Entities
{

    [Table("Users")]
    public class User : Entity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SurName { get; set; }

        public string? Patronymic { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public User(int userId, string surName, string firstName, string? patronymic, DateTime dateOfBirth, Gender gender)
        {
            UserId = userId;
            FirstName = firstName;
            SurName = surName;
            Patronymic = patronymic;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        public User(int userId, string surName, string firstName, DateTime dateOfBirth, Gender gender)
        {
            UserId = userId;
            FirstName = firstName;
            SurName = surName;
            Patronymic = null;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        public User(string surName, string firstName, string? patronymic, DateTime dateOfBirth, Gender gender)
        {
            FirstName = firstName;
            SurName = surName;
            Patronymic = patronymic;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        public User(string surName, string firstName, DateTime dateOfBirth, Gender gender)
        {
            FirstName = firstName;
            SurName = surName;
            Patronymic = null;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

    }
}
