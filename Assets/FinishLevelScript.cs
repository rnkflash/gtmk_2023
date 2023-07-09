using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class FinishLevelScript : MonoBehaviour
{
    public UnityEvent onstartfinisininginginpup;
    
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
        onstartfinisininginginpup?.Invoke();
        yield return new WaitForSeconds(1.0f);
        SceneController.Instance.Restart();
    }

    private IEnumerator winning()
    {
        onstartfinisininginginpup?.Invoke();
        yield return new WaitForSeconds(1.0f);

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
