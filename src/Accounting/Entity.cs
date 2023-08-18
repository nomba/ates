namespace Accounting;

public abstract class Entity<TId>
{
    public virtual TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    #region Identity

    public override bool Equals(object obj)
    {
        if (!(obj is Entity<TId> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (ValueObject.GetUnproxiedType(this) != ValueObject.GetUnproxiedType(other))
            return false;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id.Equals(other.Id);
    }

    public bool IsTransient()
    {
        return Id is null || Id.Equals(default(TId));
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (ValueObject.GetUnproxiedType(this).ToString() + Id).GetHashCode();
    }

    #endregion

    #region Events

    protected readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    #endregion
}

public abstract class Entity : Entity<Guid>
{
    protected Entity()
    {
    }

    protected Entity(Guid id)
        : base(id)
    {
    }
}