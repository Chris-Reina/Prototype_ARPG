using System;

[Serializable]
public class IntRange
{
    public int min = 1;
    public int max = 1;
    public int relativeMin = 0;
    public int relativeMax = 2;

    public IntRange() { }
    public IntRange(int value)
    {
        min = max = value;
        relativeMin = value - 1;
        relativeMin = value + 1;
    }
    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
        relativeMin = min;
        relativeMax = max;
    }
    public IntRange(int min, int max, int relativeMin, int relativeMax)
    {
        this.min = min;
        this.max = max;
        this.relativeMin = relativeMin;
        this.relativeMax = relativeMax;
    }

    /// <summary>
    /// Generates a random int between the parameters of the range. min [inclusive] and max [exclusive].
    /// </summary>
    /// <returns></returns>
    public int Random()
    {
        return UnityEngine.Random.Range(min, max);
    }
}
