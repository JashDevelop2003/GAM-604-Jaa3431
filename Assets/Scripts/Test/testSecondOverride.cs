using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSecondOverride : testAbsract
{
    public override void TestAbstract()
    {
        Debug.Log("I'm the second override");
    }
}
