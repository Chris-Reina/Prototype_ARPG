using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrant
{
    public Quadrant(CardinalDirection horizontal, CardinalDirection vertical)
    {
        Horizontal = horizontal;
        Vertical = vertical;
    }

    public CardinalDirection Vertical;
    public CardinalDirection Horizontal;

    public override string ToString()
    {
        return $"Quadrant: Horizontal - {Horizontal.ToString()} || Vertical - {Vertical.ToString()}";
    }
}
