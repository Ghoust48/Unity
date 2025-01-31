﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static bool isApplicationQuitting;
    private static T _instance;
    private static Object _lock = new Object();
       
    public static T Instance
    {
        get
        {
            if (isApplicationQuitting)
                return null;
                
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        var singleton = new GameObject("[SINGLETON] " + typeof(T));
                        _instance = singleton.AddComponent<T>();
                        DontDestroyOnLoad(singleton);
                    }
                }
                
                return _instance;
            }
        }
    }
    
    public virtual void OnDestroy()
    {
        isApplicationQuitting = true;
    }  
}
