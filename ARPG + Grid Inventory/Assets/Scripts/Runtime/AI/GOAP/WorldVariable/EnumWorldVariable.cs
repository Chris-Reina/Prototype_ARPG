using System;

public class EnumWorldVariable<T> : IWorldVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    
    public EnumWorldVariable(string name, T value)
    {
        Name = name;
        Value = value;
    }

    public EnumWorldVariable(EnumWorldVariable<T> other)
    {
        Name = other.Name;
        Value = other.Value;
    }
    
    public U GetValue<U>()
    {
        return (U) Value;
    }

    public T GetValue()
    {
        return (T) Value;
    }

    public IWorldVariable Clone()
    {
        return new EnumWorldVariable<T>(this);
    }
    
    protected bool Equals(EnumWorldVariable<T> other)
    {
        return Name == other.Name && Equals(Value, other.Value);
    }

    public bool Equals(IWorldVariable other)
    {
        if (other is EnumWorldVariable<T> otherB)
        {
            return Name == otherB.Name && Equals(Value, otherB.Value);
        }

        return false;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EnumWorldVariable<T>) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
        }
    }
    
    public override string ToString()
    {
        return $" | {Name} - {GetValue()}";
    }
}
    
