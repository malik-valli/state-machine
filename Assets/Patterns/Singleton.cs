using UnityEngine;

/// <summary> It has method InitializeInstance. This should be done in Awake </summary>
public static class Singleton<T> where T : MonoBehaviour
{
    public static void InitializeInstance(ref T instance, T _this)
    {
        if (instance != null && instance != _this)
            MonoBehaviour.Destroy(_this.gameObject);
        else
            instance = _this;
    }
}