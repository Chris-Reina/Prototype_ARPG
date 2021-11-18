using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestItem
{
    public Vector2Int size;
    public Color color;

    public TestItem()
    {
        size = new Vector2Int(Random.Range(1, 3), Random.Range(1, 5));
        color = Random.ColorHSV(0.0f, 1f, 0.0f, 1f, 0.0f, 1f, 1f, 1f);
    }
}
