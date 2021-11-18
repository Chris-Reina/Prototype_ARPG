using System;

public class IntWorldVariable : IWorldVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    
    public IntWorldVariable(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public IntWorldVariable(IntWorldVariable other)
    {
        Name = other.Name;
        Value = other.Value;
    }

    public T GetValue<T>()
    {
        return (T) Value;
    }

    public int GetValue()
    {
        return (int) Value;
    }
    
    public IWorldVariable Clone()
    {
        return new IntWorldVariable(this);
    }

    protected bool Equals(IntWorldVariable other)
    {
        return Name == other.Name && Equals(Value, other.Value);
    }

    public bool Equals(IWorldVariable other)
    {
        if (other is IntWorldVariable otherB)
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
        return Equals((IntWorldVariable) obj);
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
    
