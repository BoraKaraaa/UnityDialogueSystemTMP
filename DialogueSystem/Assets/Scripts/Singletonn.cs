using UnityEngine;

public abstract class Singletonn<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null) 
                {
                    Debug.Log( $"Singleton Object {typeof(T).FullName} can not found in scene");
                }
            }
            return _instance;
        }
    }
}

public abstract class SingletonnPersistent<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    
    public virtual void Awake ()
    {
        if (Instance == null) 
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        } 
        else 
        {
            Destroy(this.gameObject);
        }
    }
}
