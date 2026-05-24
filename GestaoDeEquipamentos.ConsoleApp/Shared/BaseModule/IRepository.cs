namespace GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

public interface IRepository<T> where T : BaseEntity<T>
{
    IEnumerable<T> Entities { get; }
    int Count { get; }
    bool HasEntities { get; }
    void Add(T entity);
    bool Edit(Guid id, T updatedEntity);
    bool Remove(Guid id);
    T? GetById(Guid id);
    IEnumerable<T> Where(Predicate<T> filter);
}
