using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testOverride : testAbsract
{
    public override void TestAbstract()
    {
        Debug.Log("I'm the first override");
    }
}
