namespace GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity<T>
{
    protected JsonContext context;
    protected readonly Dictionary<Guid, T> entities = [];
    public IEnumerable<T> Entities => entities.Values;
    public int Count => entities.Count;
    public bool HasEntities => Count != 0;

    public BaseRepository(JsonContext context)
    {
        this.context = context;
        entities = LoadContext();

    }

    public abstract Dictionary<Guid, T> LoadContext();

    public void Add(T entity)
    {
        entity.Id = Guid.NewGuid();
        entities.Add(entity.Id, entity);
        context.Save();
    }

    public bool Edit(T? entity, T updatedEntity)
    {
        if (entity is null || !Entities.Contains(entity))
            return false;
        entity.UpdateEntity(updatedEntity);
        context.Save();
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
        context.Save();
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
    public IEnumerable<T> Where(Predicate<T> filter)
    {
        return Entities.Where(e => filter(e));
    }
}

