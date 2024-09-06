using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class CreateUserViewModel
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public User CreateUser()
        {
            return new User(name: Name, password: Password, email: Email, phone: Phone);
        }

        public User UpdateUser(User user)
        {
            user.Name = this.Name;
            user.Password = this.Password;
            user.Email = this.Email;
            user.Phone = this.Phone;
            user.LastUpdatedAt = DateTime.UtcNow;
            return user;
        }
    }
}
