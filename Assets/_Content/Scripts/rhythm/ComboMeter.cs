using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Content.Scripts.rhythm
{
    public class ComboMeter : MonoBehaviour
    {
        [SerializeField] private TMP_Text comboLevelText;
        public Image[] images;

        private int value = -1;

        private void Start()
        {
            SetValue(0, false);
        }

        public void SetValue(int newValue, bool anim = true)
        {
            if (value == newValue) return;

            if (anim)
            {
                string[] ohnoes = new[]
                {
                    "Crash!", "Oh no!", "Whaaat?", "OMG", "-_-", "Whatever"
                };
                comboLevelText.text = value < newValue ? "LEVEL UP!" : ohnoes[Random.Range(0,ohnoes.Length)];
                comboLevelText.color = value < newValue ? Color.green : Color.red;
                
                DOTween.Shake(
                    () => comboLevelText.transform.position,
                    v => comboLevelText.transform.position = v,
                    0.5f,
                    new Vector3(0, 0.05f, 0),
                    15,
                    0f,
                    false
                ).OnComplete(() =>
                {
                    comboLevelText.text = "Combo Level";
                    comboLevelText.color = Color.white;
                });
            }
            value = newValue;
            for (int i = 0; i < images.Length; i++)
            {
                if (anim)
                    images[i].DOFade(i < value ? 1f : 0f, 0.5f);
                else
                {
                    var tempColor = images[i].color;
                    tempColor.a = i <= value ? 1f : 0f;
                    images[i].color = tempColor;    
                }
            }
        }
    }
}
