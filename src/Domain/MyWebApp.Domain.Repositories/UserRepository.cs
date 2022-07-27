using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Data.Entities;

namespace MyWebApp.Domain.Repositories;

public sealed class UserRepository : IRepository<User>
{
    private readonly NorthwindContext _context;

    public UserRepository(string connectionString) => _context = new NorthwindContext(connectionString);

    public async ValueTask<IList<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async ValueTask<User> GetByIdAsync(ulong id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async ValueTask<User> GetByNameAsync(string name)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Name == name);

        return user;
    }

    public async ValueTask<IList<User>> TakeAsync(int count)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Take(count)
            .ToListAsync();

        return users;
    }

    public ValueTask<User> CreateAndSaveAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public ValueTask DeleteByIdAsync(ulong id)
    {
        throw new NotImplementedException();
    }
}