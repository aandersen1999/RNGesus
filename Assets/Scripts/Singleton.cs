using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    protected bool setToDestroy = false;
    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            setToDestroy = true;
            return;
        }
        //DontDestroyOnLoad(gameObject);
        Instance = this as T;
    }
}
