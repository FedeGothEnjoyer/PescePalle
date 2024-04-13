using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBlock : MonoBehaviour
{
    public bool isInputEnble;

    private void Start()
    {
        isInputEnble = true;
    }

    public void DisableInput()
    {
        isInputEnble = false;
    }

    public void EnableInput()
    {
        isInputEnble = true;
    }
}
