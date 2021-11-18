namespace DoaT
{
    public class PoolObject<T> where T : IPoolSpawn
    {
        public bool IsAvailable { get; set; }
        public T Object => _internal;
        
        private readonly T _internal;

        public PoolObject(T obj)
        {
            _internal = obj;
        }

        public T GetObject()
        {
            IsAvailable = false;
            return _internal;
        }
    }
}

