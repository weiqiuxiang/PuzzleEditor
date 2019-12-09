using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviorCreateDontDestory<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance = null;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogWarning(typeof(T).Name + "is Aready Exist");
        }
        else
        {
            GetInstance();
        }
    }

    public static T GetInstance()
    {
        if (Instance == null)
        {
            System.Type t = typeof(T);

            Instance = (T)FindObjectOfType(t);

            if (Instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                Instance = obj.AddComponent<T>();

            }

            if (Instance.gameObject)
            {
                DontDestroyOnLoad(Instance.gameObject);
            }
        }

        return Instance;
    }
}
