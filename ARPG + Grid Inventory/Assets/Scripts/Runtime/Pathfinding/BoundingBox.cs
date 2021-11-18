
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox
{
    private GraphGenerator _generator;
    
    public BoundingBox(GraphGenerator generator)
    {
        _generator = generator;
    }

    public Vector3[] GetVerticesPosition()
    {
        if (!_generator) return new Vector3[0];
            
        var pos = _generator.transform.position;
        var displacement = _generator.worldSpaceSize / 2;
        var array = new Vector3[8]
        {
            //Top Rect
            new Vector3(pos.x - displacement.x, pos.y + displacement.y, pos.z + displacement.z), //Top, Left, Back
            new Vector3(pos.x - displacement.x, pos.y + displacement.y, pos.z - displacement.z), //Top, Left, Front
            new Vector3(pos.x + displacement.x, pos.y + displacement.y, pos.z - displacement.z), //Top, Right, Front
            new Vector3(pos.x + displacement.x, pos.y + displacement.y, pos.z + displacement.z), //Top, Right, Back
            //Bottom Rect
            new Vector3(pos.x - displacement.x, pos.y - displacement.y, pos.z + displacement.z), //Bottom, Left, Back
            new Vector3(pos.x - displacement.x, pos.y - displacement.y, pos.z - displacement.z), //Bottom, Left, Front
            new Vector3(pos.x + displacement.x, pos.y - displacement.y, pos.z - displacement.z),  //Bottom, Right, Front
            new Vector3(pos.x + displacement.x, pos.y - displacement.y, pos.z + displacement.z) //Bottom, Right, Back
        };
        
        return array;
    }
}
