using UnityEngine;

namespace DoaT
{
    public interface IPoolSpawn
    {
        void SetParentPool<T>(T parent);
        void Activate(Vector3 position, Quaternion rotation);
        void Deactivate();
    }
}
