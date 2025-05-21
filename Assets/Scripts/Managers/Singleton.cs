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
    //This checks if there is an existing singleton
    public static T Instance
    {
        get
        {
            if (sInstance == null)
            {
                //this finds any component that is generic to this singleton
                sInstance = FindFirstObjectByType<T>();

                //if there that manager doesn't have singleton script then create a singleton and add the manager component in
                //rename the singleton to that manager
                if(sInstance == null)
                {
                    GameObject singletonObject = new GameObject();
                    sInstance = singletonObject.AddComponent<T>();

                    //this prevents destroying the manager that is referrencing and using
                    singletonObject.name = typeof(T).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return sInstance;
        }
    }
    
    // this occurs to every component that is inheriting fro mthe awake method
    // Each manager will check if there are more than 2 identical manager components
    protected virtual void Awake()
    {
        //this checks when awake if there is an exisitng singleton when creating a new one
        //If there is then create the singleton as the manager component
        if (sInstance == null)
        {
            sInstance = this as T;
        }
        //otherwise if the new manager is not the existing manager then destroy this singleton and replace it with the new manager.
        else if (sInstance != this)
        { 
            Destroy(gameObject);
        }

    }
}
