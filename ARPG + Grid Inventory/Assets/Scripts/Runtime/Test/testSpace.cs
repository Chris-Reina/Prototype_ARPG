using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSpace : MonoBehaviour
{
    private RectTransform rectT => transform as RectTransform;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            var temp = RectTransformToScreenSpace(transform as RectTransform);
            Debug.Log(temp);
        }

        var array = new Vector3[4];
        rectT.GetWorldCorners(array);

        
    }
    
    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }
}


