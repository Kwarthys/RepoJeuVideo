using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToogler : AbstractInteract
{
    public GameObject canvas;

    private bool state = false;

    public override void execute()
    {
        state = !state;
        canvas.SetActive(state);
    }

    public void show()
    {
        if (!state)
        {
            execute();
        }
    }

    public void hide()
    {
        if (state)
        {
            execute();
        }
    }
}
