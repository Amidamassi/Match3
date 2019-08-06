using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsSelector: MonoBehaviour
{
    [SerializeField] float changeSpeed;

    private static AudioClip selects;
    private static AudioSource audioSource;
    private static LevelController levelController;
    private static Gem firstSelectedGem;
    private static bool selected;
    private static Vector3 gemPosition;

    private float blinkColor = 0;
    private int colorChange = 1;

    private void Start()
    {
        levelController = this.GetComponent<LevelController>();
        audioSource =this.GetComponent<AudioSource>();
        selects =(AudioClip) Resources.Load("Audio/Select") ;
        if (changeSpeed == 0)
        {
            changeSpeed = 0.04f;
        }
    }
   
    private void Update()
    {
        if (firstSelectedGem != null)
        {
            if (selected)
                {
                    if (blinkColor > 1)
                    {
                    blinkColor = 1;
                    colorChange *= -1;
                    }
                    if (blinkColor < 0)
                    {
                    colorChange *= -1;
                    blinkColor = 0;
                    }
                blinkColor += colorChange * changeSpeed;
                firstSelectedGem.ColorChange(blinkColor);
            }
        }
    }

    public static void SelectGem(Gem selectedGem)
    {
        if (!selected)
        {
            firstSelectedGem = selectedGem;
            selected = true;
            audioSource.PlayOneShot(selects,PlayerPrefs.GetFloat("Volume"));
        }
        else{
            if ((selectedGem.line != firstSelectedGem.line) & (selectedGem.column != firstSelectedGem.column))
            {
                Deselect();
                return;
            }else if((Mathf.Abs(selectedGem.column-firstSelectedGem.column)>1)| 
                    (Mathf.Abs(selectedGem.line - firstSelectedGem.line) > 1))
                  {
                    Deselect();
                    return;
                  }
            if (selectedGem == firstSelectedGem)
            {
                Deselect();
                return;
            }
            levelController.SwitchGems(firstSelectedGem, selectedGem);
            if (!levelController.CheckGemsForMatches(firstSelectedGem, selectedGem))
            {
                levelController.SwitchGems(selectedGem, firstSelectedGem);
                levelController.turnHelper.ChekForPossibleTurn();
            }
            Deselect();
        }
    }

    private static void Deselect()
    {
        selected = false;
        firstSelectedGem.ColorChange(1);
    }

    private void OnDestroy()
    {
        firstSelectedGem = null;
        selected = false;
    }

}
