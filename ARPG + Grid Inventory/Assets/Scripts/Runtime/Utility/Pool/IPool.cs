namespace DoaT
{
    public interface IPool<T> where T : IPoolSpawn
    {
        T GetObjectFromPool();
        void ReturnObjectToPool(T obj);
    }
}