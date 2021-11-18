using System;

public class FloatWorldVariable : IWorldVariable
{
    public string Name { get; set; }
    public object Value { get; set; }
    
    public FloatWorldVariable(string name, float value)
    {
        Name = name;
        Value = value;
    }
    
    public FloatWorldVariable(FloatWorldVariable other)
    {
        Name = other.Name;
        Value = other.Value;
    }

    public T GetValue<T>()
    {
        return (T) Value;
    }

    public float GetValue()
    {
        return (float) Value;
    }

    public IWorldVariable Clone()
    {
        return new FloatWorldVariable(this);
    }
    
    protected bool Equals(FloatWorldVariable other)
    {
        return Name == other.Name && Equals(Value, other.Value);
    }

    public bool Equals(IWorldVariable other)
    {
        if (other is FloatWorldVariable otherB)
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
        return Equals((FloatWorldVariable) obj);
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
    
