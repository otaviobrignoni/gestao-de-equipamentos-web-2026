namespace GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity<T>
{
    protected readonly Dictionary<Guid, T> entities = [];
    public IEnumerable<T> Entities => entities.Values;
    public int Count => entities.Count;
    public bool HasEntities => Count != 0;

    public void Add(T entity)
    {
        entity.Id = Guid.NewGuid();
        entities.Add(entity.Id, entity);
    }

    public bool Edit(T? entity, T updatedEntity)
    {
        if (entity is null || !Entities.Contains(entity))
            return false;

        entity.UpdateEntity(updatedEntity);
        return true;
    }
    public bool Edit(Guid id, T updatedEntity)
    {
        return Edit(GetById(id), updatedEntity);
    }
    public bool Remove(T? entity)
    {
        if (entity is null)
            return false;
        if (!entities.Remove(entity.Id))
            return false;

        return true;
    }
    public bool Remove(Guid id)
    {
        return Remove(GetById(id));
    }
    public T? GetById(Guid id)
    {
        return entities.GetValueOrDefault(id);
    }
    public IEnumerable<T> GetAllExcept(IEnumerable<T>? ignoredEntities = null)
    {
        ignoredEntities ??= [];
        return Entities.Except(ignoredEntities);   
    }
}

