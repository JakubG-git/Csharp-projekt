using System.ComponentModel.DataAnnotations;

namespace TMS.Models;

public class RegisterModelRole
{
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string Role { get; set; }
    
}