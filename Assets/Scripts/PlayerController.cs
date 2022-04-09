using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public event Action Action;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            Action?.Invoke();
        }
    }
}
