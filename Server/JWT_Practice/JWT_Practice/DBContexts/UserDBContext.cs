using JWT_Practice.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Practice.DBContexts;

public class UserDBContext : DbContext
{
    public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}