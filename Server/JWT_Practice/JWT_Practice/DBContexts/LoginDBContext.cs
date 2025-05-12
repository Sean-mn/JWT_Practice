using JWT_Practice.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Practice.DBContexts;

public class LoginDBContext : DbContext
{
    public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}