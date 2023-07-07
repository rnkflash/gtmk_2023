using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Content.Scripts.scenes
{
    public class IntroController : MonoBehaviour
    {
        public Image bg;
        private bool waitForKeyPress = false;
    
        void Start()
        {
            bg.DOFade(0.0f, 1.0f).OnComplete(() =>
            {
                waitForKeyPress = true;
            });
        }

    
        void Update()
        {
            if (!waitForKeyPress)
                return;
            if (Input.anyKey)
            {
                bg.DOFade(1.0f, 1.0f).OnComplete(() =>
                {
                    SceneController.Instance.Load("menu");
                });
                waitForKeyPress = false;
            }
        }
    }
}
