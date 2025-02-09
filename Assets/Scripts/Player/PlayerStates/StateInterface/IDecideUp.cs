using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an interface which ensure that when inherting must provide the void
/// This interface must only be used when the state requires an option to choose up
/// </summary>

public interface IDecideUp
{
    public void DecidingUp(object sender, EventArgs e)
    {
        
    }
}
