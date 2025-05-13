using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Practice.Models;

[Table("user")]
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}