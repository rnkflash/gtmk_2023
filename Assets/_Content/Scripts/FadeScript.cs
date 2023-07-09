using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public Image bg;

    public void FadeIn()
    {
        bg.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            
        });
    }
    
    public void FadeOut()
    {
        bg.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            
        });
    }
}
