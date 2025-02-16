using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceBehaviour : MonoBehaviour
{
    [SerializeField] private spaceEnum spaceType;
    public spaceEnum SpaceType { get { return spaceType; } }
}
