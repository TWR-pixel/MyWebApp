using MyWebApp.Data.Entities;
using MyWebApp.Domain.Repositories;
using MyWebApp.Domain.Exceptions;

namespace MyWebApp.Domain.Services;

public sealed class GroupService
{
    private readonly IRepository<Group> _repo;

    public GroupService(IRepository<Group> repo) => _repo = repo;

    public GroupService(string connectionString) => _repo = new GroupRepository(connectionString);

    /// <summary>
    /// Возвращает все группы
    /// </summary>
    /// <returns></returns>
    public async ValueTask<IList<Group>> GetAllAsync() => await _repo.GetAllAsync();

    /// <summary>
    /// Создает и сохраняет группу
    /// </summary>
    /// <param name="group">группа</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">если group является null</exception>
    public async ValueTask<Group> CreateAsync(Group group)
    {
        if (group is null) throw new ArgumentNullException(nameof(group));

        return await _repo.CreateAndSaveAsync(group);
    }

    /// <summary>
    /// Возвращает группу
    /// </summary>
    /// <param name="id">id группы</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">Если группа не найдена по id</exception>
    public async ValueTask<Group> GetById(ulong id)
    {
        var group = await _repo.GetByIdAsync(id);

        if (group is null) throw new EntityNotFoundException();

        return group;
    }

    /// <summary>
    /// Удаляет группу по id
    /// </summary>
    /// <param name="id">id группы</param>
    /// <returns></returns>
    public async ValueTask DeleteByIdAsync(ulong id) => await _repo.DeleteByIdAsync(id);
}