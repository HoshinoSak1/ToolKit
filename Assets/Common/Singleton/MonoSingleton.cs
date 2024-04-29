using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;
    public static bool destroyOnLoad = false;
    public static GameObject monoSingleton;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                // 如果场景中不存在实例，则创建一个新的游戏对象并将该组件附加到它上面
                if (instance == null)
                {
                    monoSingleton = new GameObject(typeof(T).Name);
                    instance = monoSingleton.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    public virtual void Init(bool needDestroy = false)
    {
        destroyOnLoad = needDestroy;
        if (needDestroy)
        {
            AddSceneChangedEvent();
        }
    }

    public void AddSceneChangedEvent()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        if (destroyOnLoad)
        {
            if (!instance)
            {
                DestroyImmediate(instance);
            }
        }
    }
}
