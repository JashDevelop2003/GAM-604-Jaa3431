using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is the singleton design pattern that provides generic and persitent impementations
/// This is used for preventing each singleton to be destoryed when changing scenes
/// </summary>
/// <typeparam name="T"> Should be a MonoBehaviour of a singleton manager </typeparam>

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T sInstance;
    public static T Instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = FindFirstObjectByType<T>();

                if(sInstance == null)
                {
                    GameObject singletonObject = new GameObject();
                    sInstance = singletonObject.AddComponent<T>();

                    singletonObject.name = typeof(T).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return sInstance;
        }
    }
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if (sInstance == null)
        {
            sInstance = this as T;
        }
        else if (sInstance != this)
        { 
            Destroy(gameObject);
        }

    }
}
