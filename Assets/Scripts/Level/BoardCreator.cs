using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LevelController))]
public class BoardCreator : MonoBehaviour
{
    [SerializeField] LevelController levelController;
    [SerializeField] Gem gemPrefab;
    [SerializeField] GameObject background;
    [SerializeField] Vector2 startPosition;

    public Gem[,] CreateBoard(Level level)
    {
        Gem[,] gems = new Gem[level.boardHeight, level.boardWidth];
        int k = 0;
        for (int i = 0; i < level.boardWidth; i++)
        {
            for (k = 0; k < level.boardHeight; k++)
            {
                gems[k, i] = Instantiate(gemPrefab, new Vector3(startPosition.x + i * 0.64f, startPosition.y - k * 0.64f),
                                       Quaternion.identity, this.transform);
                gems[k, i].line = k;
                gems[k, i].column = i;
                gems[k, i].targetPosition = gems[k, i].transform.position;

                if (level.board[k + i * level.boardHeight] == 0)
                {
                    levelController.RandomColor();
                    gems[k, i].ChangeType(levelController.randomType, levelController.gemsSprites[levelController.randomType]);
                    Instantiate(background, new Vector3(startPosition.x + i * 0.64f, startPosition.y - k * 0.64f),
                                Quaternion.identity, this.transform);
                }
                else
                {
                    gems[k, i].ChangeType(-1, null);
                    gems[k, i].gameObject.SetActive(false);
                    Instantiate(background, new Vector3(startPosition.x + i * 0.64f, startPosition.y - k * 0.64f),
                        Quaternion.identity, this.transform).gameObject.SetActive(false);
                }
            }
        }
        return gems;
    }
}
