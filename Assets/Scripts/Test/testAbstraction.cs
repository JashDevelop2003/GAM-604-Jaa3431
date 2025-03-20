using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAbstraction : MonoBehaviour
{
    [SerializeField] private GameObject testObjectOne;
    [SerializeField] private GameObject testObjectTwo;
    //private int intAbstract = 1;

    [SerializeField] private testAbsract changeAbstraction;

    private void Start()
    {
        changeAbstraction = testObjectOne.GetComponent<testAbsract>();
        changeAbstraction.TestAbstract();
        StartCoroutine(ChangeAbstraction());
    }


    IEnumerator ChangeAbstraction()
    {
        yield return new WaitForSeconds(1);
        Changeing();
    }

    void Changeing()
    {
        changeAbstraction = testObjectTwo.GetComponent<testAbsract>();
        changeAbstraction.TestAbstract();
    }

}
