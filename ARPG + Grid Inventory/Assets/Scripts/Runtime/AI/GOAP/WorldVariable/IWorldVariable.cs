
using System;


public interface IWorldVariable : IEquatable<IWorldVariable>
{
    string Name { get; set; }
    object Value { get; set; }

    T GetValue<T>();
    IWorldVariable Clone();
}    
