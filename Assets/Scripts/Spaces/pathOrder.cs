using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pathOrder : MonoBehaviour
{
    [SerializeField] private directionEnum direction;
    [SerializeField] private List <GameObject> spaceOrder;
    
    public directionEnum Direction { get { return direction; } }
    public List<GameObject> SpaceOrder {  get { return spaceOrder; } }
    
    //[SerializeField] private GameObject directionalSpace;

    //void Start()
    //{
    //    foreach(Transform space in transform)
    //    {
    //        spaceOrder.Add(space.gameObject); 
    //    }
    //    spaceOrder.Add (directionalSpace.gameObject);
    //}
}
