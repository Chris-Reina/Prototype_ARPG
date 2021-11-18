using DoaT.AI;

public interface IPath : IPositionProperty
{
    int CurrentIndex { get; set; }
    Path Path { get; set; }
}
