using UnityEngine;

public class SingletonTest : MonoBehaviour
{
    public static SingletonTest instance;

    private void Awake()
    {
        instance = this;
    }

    public void Hello()
    {
        Debug.Log("SingletonOK");
    }
}
