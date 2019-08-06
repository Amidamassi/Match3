using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image levels;
    public void OpenLevels()
    {
        levels.gameObject.SetActive(true);
    }
    public void CloseLevels()
    {
        levels.gameObject.SetActive(false);
    }
}
