using System;

[Serializable]
public class FloatRange
{
    public float min = 0.5f;
    public float max = 0.5f;
    public float relativeMin = 0f;
    public float relativeMax = 1f;

    public FloatRange() { }
    public FloatRange(float value)
    {
        min = max = value;
        relativeMin = value - 1;
        relativeMin = value + 1;
    }
    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
        relativeMin = min;
        relativeMax = max;
    }
    public FloatRange(float min, float max, float relativeMin, float relativeMax)
    {
        this.min = min;
        this.max = max;
        this.relativeMin = relativeMin;
        this.relativeMax = relativeMax;
    }

    /// <summary>
    /// Generates a random float between the parameters of the range. min [inclusive] and max [inclusive].
    /// </summary>
    /// <returns></returns>
    public float Random()
    {
        return UnityEngine.Random.Range(min, max);
    }
}
