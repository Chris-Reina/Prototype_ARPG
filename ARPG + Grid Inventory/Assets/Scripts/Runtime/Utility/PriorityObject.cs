namespace DoaT
{
    [System.Serializable]
    public class PriorityObject<T>
    {
        public T storedObject;
        public float priorityValue;

        public PriorityObject(T objectToStore, float value)
        {
            storedObject = objectToStore;
            priorityValue = value;
        }
    }
}
