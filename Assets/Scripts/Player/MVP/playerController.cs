using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private playerModel playerModel;
    [SerializeField] private characterData Data;
    public characterData GetData {  get { return Data; } }
    
    // Start is called before the first frame update
    void Awake()
    {
        playerModel = new playerModel(Data);
    }

}
