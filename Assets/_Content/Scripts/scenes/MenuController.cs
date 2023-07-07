using UnityEngine;
using UnityEngine.UIElements;

namespace _Content.Scripts.scenes
{
    public class MenuController : MonoBehaviour
    {

        private Button startButton;
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            startButton = root.Q<Button>("ButtonStart");
            startButton.clicked += OnStartClicked;

        }

        private void OnDisable()
        {
            startButton.clicked -= OnStartClicked;
        }

        private void OnStartClicked()
        {
            SceneController.Instance.Load("game");
        }
    }
}
