using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SongAuthorIntro : MonoBehaviour
{
    private TMPro.TMP_Text text;
    
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.alpha = 0;
    }

    public void Go()
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
        
        text.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            text.DOFade(0.0f, 1.0f).OnComplete(() =>
            {
            
            });
        });
    }
}
