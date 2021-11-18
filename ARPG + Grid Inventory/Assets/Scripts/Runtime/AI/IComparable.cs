namespace DoaT.AI
{
    public interface IComparable<T>
    {
        double Priority { get; }
        
        int CompareTo(T other);
    }
}