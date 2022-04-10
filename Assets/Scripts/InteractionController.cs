using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance;

    public event Action Action;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            Action?.Invoke();
        }
    }
}
