using UnityEngine;
using UnityEngine.Events;

public class StartLevelScript : MonoBehaviour
{

    public UnityEvent onStart;
    
    
    void Start()
    {
        onStart?.Invoke();
    }
}
