using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHelper : MonoBehaviour
{
    [SerializeField] public GameObject possibleTurn;
    [SerializeField] float timeForHelp;

    private LevelController levelController;
    private Gem gem;
    private Gem gemBuffer;
    private int[] maskForCheckRow = new int[] { 0, 1, - 1,  1, - 1, 0 };
    private int[] maskForCheckRow2 = new int[] {- 2, - 1, - 1, + 2, + 2, + 3};
    private int[] maskForBetween = new int[] { -1, 1 };
    private int[] maskForBetween2 = new int[] { 1, 1 };

    private void Start()
    {
        possibleTurn = Instantiate(possibleTurn, this.transform);
        possibleTurn.gameObject.SetActive(false);
        levelController = this.GetComponent<LevelController>();
    }
    public void ChekForPossibleTurn()
    {
        StopAllCoroutines();
        for (int i = 0; i < levelController.level.boardHeight-1; i++)
        {
            for (int k = 0; k < levelController.level.boardWidth-1; k++)
            {
                gem = levelController.gems[i, k];
                if (gem.type < 0 ) { continue; }
                if (gem.type > 5) {return;}
                if (ChekTurns2InRow())
                {
                    gem = gemBuffer;
                    StartCoroutine(PlayerHelp());
                    return;
                }
                if (ChekTurnsBetween())
                {
                    gem = gemBuffer;
                    StartCoroutine(PlayerHelp());
                    return;
                }
            }
        }
        levelController.MixBoard();
    }
    
    private bool Chek(int line,int column,int type)
    {
        if((line > 0) & (line < levelController.level.boardHeight) & (column >0) & 
                    (column < levelController.level.boardWidth))
        {
            if (levelController.gems[line, column].type == type){ return true;}
            else{ return false;}
        }
        return false;
    }
    private bool ChekTurns2InRow()
    {
        if (Chek(gem.line, gem.column + 1, gem.type))
        {
            if ((gem.column>0)&&(levelController.gems[gem.line, gem.column - 1].type != -1))
            {if (ChekTurns2InRowPart(maskForCheckRow, maskForCheckRow2, 0)) { return true; }}
            if ((gem.column < levelController.level.boardWidth-2)&&
                levelController.gems[gem.line, gem.column + 2].type != - 1)
            {if (ChekTurns2InRowPart(maskForCheckRow, maskForCheckRow2, 1)) { return true; }}
        }
        if (Chek(gem.line+1, gem.column , gem.type))
        {
            if ((gem.line>0)&&(levelController.gems[gem.line - 1, gem.column ].type != - 1))
            {if (ChekTurns2InRowPart(maskForCheckRow2, maskForCheckRow, 0)) { return true; }}
            if ((gem.line<levelController.level.boardHeight-2)&&
                (levelController.gems[gem.line + 2, gem.column ].type != - 1))
            { if (ChekTurns2InRowPart(maskForCheckRow2, maskForCheckRow, 1)) { return true; }}
        }
        return false;
    }
   
    private bool ChekTurns2InRowPart(int[] mask1,int[] mask2,int part)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Chek(gem.line + mask1[i+3*part], gem.column + mask2[i+3*part], gem.type)){
                gemBuffer = levelController.gems[gem.line + mask1[i + 3 * part], gem.column + mask2[i + 3 * part]];
                return true;
            }
        }
        return false;
    }
    private bool ChekTurnsBetween()
    {
        if (Chek(gem.line, gem.column + 2, gem.type))
        {
            if (levelController.gems[gem.line, gem.column + 1].type != -1)
            {if(ChekTurnsBetweenPart(maskForBetween, maskForBetween2)) { return true; } }
        }
        if (Chek(gem.line + 2, gem.column, gem.type))
        {
            if (levelController.gems[gem.line+1, gem.column].type != -1)
            {if (ChekTurnsBetweenPart(maskForBetween2, maskForBetween)) { return true; }}
        }
        return false;
    }

    private bool ChekTurnsBetweenPart(int[] mask1, int[] mask2)
    {
        for (int i = 0; i < 2; i++)
        {
            if (Chek(gem.line + mask1[i], gem.column + mask2[i], gem.type)){
                gemBuffer=levelController.gems[gem.line + mask1[i], gem.column + mask2[i]];
                return true;}
        }
        return false;
    }

    IEnumerator PlayerHelp()
    {
        yield return new WaitForSeconds(timeForHelp);
        possibleTurn.transform.position = gem.targetPosition;
        possibleTurn.SetActive(true);
    }
}
