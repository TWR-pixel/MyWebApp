using MyWebApp.Data.Entities;
using MyWebApp.Domain.Repositories;
using MyWebApp.Domain.Exceptions;

namespace MyWebApp.Domain.Services;

public sealed class GroupService
{
    private readonly IRepository<Group> _repo;

    public GroupService(IRepository<Group> repo) => _repo = repo;

    public GroupService(string connectionString) => _repo = new GroupRepository(connectionString);

    public async ValueTask<IList<Group>> GetAllAsync() => await _repo.GetAllAsync();

    public async ValueTask<Group> CreateAsync(Group group)
    {
        if (group is null) throw new ArgumentNullException(nameof(group));

        return await _repo.CreateAsync(group);
    }

    public async ValueTask<Group> GetById(ulong id)
    {
        var group = await _repo.GetByIdAsync(id);

        if (group is null) throw new EntityNotFoundException();

        return group;
    }
}   