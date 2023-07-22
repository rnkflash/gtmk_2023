using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinAfterTenSeconds : MonoBehaviour
{
    public UnityEvent onWin;
    void Start()
    {
        StartCoroutine(winning());
    }

    private IEnumerator winning()
    {
        yield return new WaitForSeconds(10.0f);
        onWin?.Invoke();
    }
}
