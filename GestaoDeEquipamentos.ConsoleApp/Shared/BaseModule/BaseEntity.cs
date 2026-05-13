namespace GestaoDeEquipamentos.ConsoleApp.Shared.BaseModule;

public abstract class BaseEntity<T> where T : BaseEntity<T>
{
    public Guid Id { get; set; }
    public abstract void UpdateEntity(T updatedEntity);
    public abstract bool Equals(T entity);
    public override bool Equals(object? obj) => obj is T entity && Equals(entity);
    public override int GetHashCode() => Id.GetHashCode();
}