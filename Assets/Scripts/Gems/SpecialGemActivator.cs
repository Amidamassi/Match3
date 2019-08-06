using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGemActivator : MonoBehaviour
{
    [SerializeField] Bomb bombPrefab;
    [SerializeField] Rocket rocketPrefab;

    private static Rocket[] rockets;
    private static Bomb[] bombs;
    private static Gem gemBuffer;
    private static LevelController levelController;
    private static SpecialGemActivator specialGemActivator;

    private void Start()
    {
        specialGemActivator = this;
        rockets = new Rocket[10];
        for(int i = 0; i < 10; i++)
        {
            rockets[i] = Instantiate(rocketPrefab);
            rockets[i].gameObject.SetActive(false);
        }
        bombs = new Bomb[4];
        for(int i = 0; i < 4; i++)
        {
            bombs[i] = Instantiate(bombPrefab);
            bombs[i].gameObject.SetActive(false);
            
        }
        levelController = this.GetComponent<LevelController>();
    }
    public static void ActivateGem(Gem gem)
    {
        levelController.turnHelper.possibleTurn.SetActive(false);
        switch (gem.type)
        {
            case 5:
                RocketActivate(gem);
                break;
            case 6:
                BombActivate(gem);
                break;
        }
        
    }
    private static void RocketActivate(Gem gem)
    {
        for(int i=0; i < 5; i++)
        {
            while (true) {

                if ((gemBuffer = levelController.gems[Random.Range(0, levelController.level.boardHeight),
                                                 Random.Range(0, levelController.level.boardWidth)]).type > -1)
                {
                    break;
                }
            }
            if (gemBuffer.type < 5)
            {
                ActivateRocket(gemBuffer.targetPosition,gem);
                levelController.gemsForDisable.Add(gemBuffer);
            }
            else
            {
                ActivateRocket(gemBuffer.targetPosition, gem);
                gemBuffer.ActivateGem();
            }
        }
        levelController.StartCoroutine(levelController.Clear());
    }
    private static void ActivateRocket(Vector3 target,Gem gem)
    {
        for(int i = 0; i < 10; i++)
        {
            if (rockets[i].gameObject.activeInHierarchy == false)
            {
                rockets[i].transform.position = gem.targetPosition;
                rockets[i].targetPosition = target;
                rockets[i].image.gameObject.SetActive(true);
                rockets[i].gameObject.SetActive(true);
                return;
            }
        }
        specialGemActivator.StartCoroutine(specialGemActivator.Extra(target,gem));
    }
    IEnumerator Extra(Vector3 target, Gem gem)
    {
        Rocket rocket;
        rocket = Instantiate(rocketPrefab);
        rocket.transform.position = gem.targetPosition;
        rocket.targetPosition = target;
        yield return new WaitForSeconds(1f);
        Destroy(rocket);
    }
    private static void BombActivate(Gem gem)
    {
        for(int i = 0; i < 3; i++)
        {
            for(int k=0; k < 3; k++)
            {
                if ((gem.line + i > 0) && (gem.line + i - 1 < levelController.level.boardHeight)&&
                    (gem.column + k > 0) && (gem.column + k - 1 < levelController.level.boardWidth))
                {
                    gemBuffer = levelController.gems[gem.line + i - 1, gem.column + k - 1];
                    
                    levelController.gemsForDisable.Add(gemBuffer);
                }
            }
        }
        ActivateBomb(gem.targetPosition);
        levelController.StartCoroutine(levelController.Clear());
    }
    private static void ActivateBomb(Vector3 target)
    {
        for(int i = 0; i < 4; i++)
        {
            if (!bombs[i].gameObject.activeInHierarchy)
            {
                bombs[i].transform.position = target;
                bombs[i].gameObject.SetActive(true);
                bombs[i].Explode();
                break;
            }
        }
    }
}
