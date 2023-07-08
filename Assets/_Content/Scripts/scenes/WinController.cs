using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneController.Instance.Load("menu");
    }
}
