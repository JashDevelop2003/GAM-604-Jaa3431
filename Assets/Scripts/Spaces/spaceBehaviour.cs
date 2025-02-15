using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceBehaviour : MonoBehaviour
{
    [SerializeField] private spaceEnum spaceType;
    [SerializeField] private Transform nextSpace;

    public spaceEnum SpaceType { get { return spaceType; } }
    public Transform NextSpace { get { return nextSpace; } }
}
