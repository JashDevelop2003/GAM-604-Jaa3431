using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rookX : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] int speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPosition.transform.position;
    }

    public void MoveRook()
    {
        transform.position = startPosition.transform.position;
        StartCoroutine(EndRook());
    }

    IEnumerator EndRook()
    {
        while (transform.position != endPosition.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
