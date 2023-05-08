using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputActionListener : MonoBehaviour
{
    public UnityEvent OnPause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause.Invoke();
        }
    }
}
