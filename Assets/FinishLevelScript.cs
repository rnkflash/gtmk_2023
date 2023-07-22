using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class FinishLevelScript : MonoBehaviour
{
    public UnityEvent onWin;
    public UnityEvent onLose;
    
    public void Win()
    {
        StartCoroutine(winning());
    }

    public void Lose()
    {
        StartCoroutine(losing());
    }

    private IEnumerator losing()
    {
        onLose?.Invoke();
        yield return new WaitForSeconds(3.0f);
        SceneController.Instance.Restart();
    }

    private IEnumerator winning()
    {
        onWin?.Invoke();
        yield return new WaitForSeconds(3.0f);

        Player.Instance.currentLevel++;

        if (Player.Instance.currentLevel >= Player.Instance.levels.Length)
        {
            SceneController.Instance.Load("menu");
        }
        else
        {
            SceneController.Instance.Load(Player.Instance.getCurrentLevel().sceneName);
        }
    }
}
