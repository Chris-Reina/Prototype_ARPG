using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFeedback : MonoBehaviour //WIP
{
    public new AnimationClip animation;


    private float timer;
    private void Start()
    {
        timer = animation.length;
    }

    private void Update()
    {
        if (timer <= 0)
            Destroy(gameObject);
        else
            timer -= Time.deltaTime;
    }
}
