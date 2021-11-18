using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public class CursorFeedbackManager : MonoBehaviour
{
    public CursorFeedback prefab;

    public float yDisplacement;

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnWorldClick,InstantiateFeedback);
    }

    private void InstantiateFeedback(params object[] parameters)
    {
        var temp = Instantiate(prefab);

        temp.transform.position = (Vector3) parameters[0] + new Vector3(0, yDisplacement, 0);
    }
}
