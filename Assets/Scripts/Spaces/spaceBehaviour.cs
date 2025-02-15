using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceBehaviour : MonoBehaviour
{
    [SerializeField] private spaceEnum spaceType;
    [SerializeField] private GameObject nextSpace;

    public spaceEnum SpaceType { get { return spaceType; } }
    public GameObject NextSpace { get { return nextSpace; } }
}
