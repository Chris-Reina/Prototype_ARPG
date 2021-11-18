namespace DoaT
{
    public interface IFactory<out T>
    {
        T Create(object obj);
    }
}