using MyWebApp.Data.Entities;
using MyWebApp.Domain.Exceptions;
using MyWebApp.Domain.Repositories;

namespace MyWebApp.Domain.Services;

public sealed class UserService
{
    private readonly UserRepository _repo;

    public UserService(string connectionString) => _repo = new UserRepository(connectionString);

    /// <summary>
    /// возвращает пользователя по id
    /// </summary>
    /// <param name="id">id пользователя</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">если пользователь по id не найден</exception>
    public async ValueTask<User> GetByIdAsync(ulong id)
    {
        var user = await _repo.GetByIdAsync(id);

        if (user is null) throw new EntityNotFoundException();

        return user;
    }

    /// <summary>
    /// получает пользователя по имени
    /// </summary>
    /// <param name="name">имя пользователя</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async ValueTask<User> GetByNameAsync(string name)
    {
        var user = await _repo.GetByNameAsync(name);

        if (user is null) throw new EntityNotFoundException();

        return user;
    }
}