public interface IFactory<T>
{
    T Create(object obj);
}
