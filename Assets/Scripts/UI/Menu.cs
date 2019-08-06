using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] Image menu;
    [SerializeField] Image winMenu;

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
