using System.ComponentModel.DataAnnotations;

namespace MoviePickerInfrastructure.Models;

public class RegisterViewModel
{

    [Required]
    [DataType(DataType.Text)]
    public string Login { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    [DataType(DataType.Password)]
    //passsword
    public string Password { get; set; }

    [Required]
    //password confirmation
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
}
