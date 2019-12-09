using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T: MonoBehaviour
{
    public static T Instance = null;

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);

            Debug.LogWarning(typeof(T).Name + " is Aready Exist" );
        }
    }

    public static T GetInstance()
    {
        if(Instance == null)
        {
            System.Type t = typeof(T);

            Instance = (T)FindObjectOfType(t);

            if(Instance == null)
            {
                Debug.LogWarning(t.Name + " is no attach on any GameObject");
            }
        }

        return Instance;
    }
}
