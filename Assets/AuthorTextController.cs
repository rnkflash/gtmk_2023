using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AuthorTextController : MonoBehaviour
{
    public TMP_Text text;
    
    void Start()
    {
        text.alpha = 0;
        
    }

    public void Go()
    {
        StartCoroutine(winning());
    }

    private IEnumerator winning()
    {
        text.alpha = 0;
        var level = Player.Instance.getCurrentLevel();
        if (level != null)
        {
            text.text = level.author + " - " + level.song;
        }
        else
        {
            text.text = "Unknown Author - Unknown Song";
        }
        yield return new WaitForSeconds(1.0f);
        text.DOFade(1.0f, 2.0f);
        yield return new WaitForSeconds(2.0f);
        text.DOFade(0.0f, 2.0f);

    }
}
