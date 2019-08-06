using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
[RequireComponent(typeof(BoardCreator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TurnHelper))]
[RequireComponent(typeof(ScoreController))]
public class LevelController : MonoBehaviour
{
    public Level level { get; private set; }
    public Gem[,] gems { get; private set; }
    public int randomType { get; private set; }

    [SerializeField] public TurnHelper turnHelper;
    [SerializeField] public ScoreController scoreController;
    [SerializeField] public Sprite[] gemsSprites;

    [SerializeField] BoardCreator boardCreator;
    [SerializeField] AudioClip swap;
    [SerializeField] AudioClip clear;
    [SerializeField] AudioSource audioSource;
    
    [HideInInspector] public List<Gem> gemsForDisable = new List<Gem>();
    
    private int numberSpecGemToCreate=0;

    private void Start()
    {
        XmlSerializer deserializer = new XmlSerializer(typeof(Level));
        TextAsset text = (Resources.Load("XML/Level" + PlayerPrefs.GetInt("currentLevel")) as TextAsset);
        StringReader stream = new StringReader(text.text);
        level = (Level)deserializer.Deserialize(stream);
        stream.Dispose();
        audioSource = GetComponent<AudioSource>();
        turnHelper = this.GetComponent<TurnHelper>();
        gems = boardCreator.CreateBoard(level);
        CheckBoardforMathes();
        StartCoroutine(Clear());
        scoreController.Initialize(level.scoreLevel);
    }
 
    public bool CheckGemsForMatches(Gem gem1, Gem gem2)
    {
        audioSource.PlayOneShot(swap, PlayerPrefs.GetFloat("Volume") / 4);
        turnHelper.possibleTurn.SetActive(false);
        if( CheckGemForMatches(gem2) | CheckGemForMatches(gem1))
        {
            scoreController.nextTurn();
            return true;
        }
        return false;
    }
    private bool CheckGemForMatches(Gem gem)
    {
        CheckColumnForMatches(gem.column);
        CheckLineForMatches(gem.line);
        if (gemsForDisable.Count==0)
        {
            return false;
        }
        ClearMatches();
        CreateSpecificGem();
        turnHelper.ChekForPossibleTurn();
        return true;
    }

    private void CheckColumnForMatches(int Number)
    {
        int countInRow=1;
        for (int i = 0; i < level.boardHeight-1; i++)
        {
            if (gems[i, Number].gameObject.activeInHierarchy)
            {
                if (gems[i, Number].type == gems[i + 1, Number].type)
                {
                    countInRow++;
                }else{
                    if (countInRow > 2)
                    {
                        for (int k = 0; k < countInRow; k++)
                        {
                            gemsForDisable.Add(gems[i - k, Number]);
                        }
                    }
                    if (countInRow > 3) { numberSpecGemToCreate++; }
                    countInRow = 1;
                }
            }     
        }
        if (countInRow > 2)
        {
            for (int k = 0; k < countInRow; k++)
            {
                gemsForDisable.Add(gems[level.boardHeight - 1 - k, Number]);
            }
        }
        if (countInRow > 3) { numberSpecGemToCreate++; }
    }

    private void CheckLineForMatches(int Number)
    {
        int countInRow = 1;
        for (int i = 0; i < level.boardWidth -1; i++)
        {
            if (gems[Number, i].gameObject.activeInHierarchy)
            {
                if (gems[Number, i].type == gems[Number, i + 1].type)
                {
                    countInRow++;
                }else{
                    if (countInRow > 2)
                    {
                        for (int k = 0; k < countInRow; k++)
                        {
                            gemsForDisable.Add(gems[Number, i - k]);
                        }
                    }
                    if (countInRow > 3) { numberSpecGemToCreate++; }
                    countInRow = 1;
                }
            }
        }
        if (countInRow > 2)
        {
            for (int k = 0; k < countInRow; k++)
            {
                gemsForDisable.Add(gems[Number, level.boardWidth -1 - k]);
            }
        }
    if (countInRow > 3) { numberSpecGemToCreate++; }
    }

    public void ClearMatches()
    {
        foreach(Gem gem in gemsForDisable)
        {
            gem.gameObject.SetActive(false);
            scoreController.ChangeScore(1);
        }
        audioSource.PlayOneShot(clear, PlayerPrefs.GetFloat("Volume")/2);
        gemsForDisable = new List<Gem>();
        CheckforEmpty();
        CheckBoardforMathes();
    }

    public void CheckforEmpty()
    {
        bool allAboveEmpty=true;
        for (int i=0; i< level.boardHeight; i++)
        {
            for (int k = 0; k < level.boardWidth; k++)
                if (gems[level.boardHeight - i - 1, k].type != -1) {
                    if (!gems[level.boardHeight - 1 - i, k].gameObject.activeInHierarchy)
                    {

                        for (int j = 0; j < level.boardHeight - i; j++)
                        {
                            if (gems[level.boardHeight - i - 1 - j, k].gameObject.activeInHierarchy)
                            {
                                gems[level.boardHeight - i - 1, k].ChangeType(gems[level.boardHeight - 1 - i - j, k].type,
                                    gemsSprites[gems[level.boardHeight - 1 - i - j, k].type]);
                                gems[level.boardHeight - i - 1, k].transform.position =
                                    gems[level.boardHeight - 1 - i - j, k].transform.position;
                                gems[level.boardHeight - i - 1, k].gameObject.SetActive(true);
                                gems[level.boardHeight - 1 - i - j, k].gameObject.SetActive(false);
                                allAboveEmpty = false;
                                break;
                            } else {
                                allAboveEmpty = true;
                            }

                        }
                        if (allAboveEmpty)
                        {
                                randomType = Random.Range(0, level.colors);
                            gems[level.boardHeight - i - 1, k].ChangeType(randomType, gemsSprites[randomType]);
                            gems[level.boardHeight - i - 1, k].transform.position =
                                new Vector3(gems[level.boardHeight - i - 1, k].transform.position.x,
                                            4.5f,
                                            gems[level.boardHeight - i - 1, k].transform.position.z);
                            gems[level.boardHeight - i - 1, k].gameObject.SetActive(true);
                        }

                    }
                }
        }
    }

    public void CheckBoardforMathes()
    {
        for (int i = 0; i < level.boardHeight; i++)
        {
            CheckLineForMatches(i);
        }
        for (int i = 0; i < level.boardWidth; i++)
        {
            CheckColumnForMatches(i);
        }
        if (gemsForDisable.Count != 0)
        {
            ClearMatches();
        }
    }

    public void SwitchGems(Gem gem1, Gem gem2)
    {
        randomType = gem1.type;
        gem1.ChangeType(gem2.type, gemsSprites[gem2.type]);
        gem2.ChangeType(randomType, gemsSprites[randomType]);
    }
    
    public void MixBoard()
    {
        int randomLine;
        int randomColumn;
        for (int i = 0; i < level.boardHeight; i++)
        {
            for (int k = 0; k < level.boardWidth; k++)
            {
                if (gems[i, k].type != -1)
                    if (gems[i, k].type < 6)
                    {
                        {
                            randomLine = Random.Range(0, level.boardHeight);
                            randomColumn = Random.Range(0, level.boardWidth);
                            while (gems[randomLine, randomColumn].type == -1)
                            {
                                randomLine = Random.Range(0, level.boardHeight);
                                randomColumn = Random.Range(0, level.boardWidth);
                            }
                            SwitchGems(gems[i, k], gems[randomLine, randomColumn]);
                        }
                    }
                    else
                    {
                        RandomColor();
                        gems[i, k].ChangeType(randomType, gemsSprites[randomType]);
                    }
            }
        }
        CheckBoardforMathes();
        turnHelper.ChekForPossibleTurn();
    }
    public void Restart()
    {
        MixBoard();
        scoreController.Initialize(level.scoreLevel);
        numberSpecGemToCreate = 0;
        for (int i = 0; i < level.boardHeight; i++)
        {
            for (int k = 0; k < level.boardWidth; k++)
            {
                if (gems[i, k].type >4 )
                {
                    RandomColor();
                    gems[i, k].ChangeType(randomType, gemsSprites[randomType]);
                }
            }
        }
    }

    public void CreateSpecificGem()
    {
        Gem gembuffer;
        for (int i=0; i < numberSpecGemToCreate; i++)
        {
            while (true)
            {
                if ((gembuffer = gems[Random.Range(0, level.boardHeight), Random.Range(0, level.boardWidth)]).type < 6)
                {
                    if(gembuffer.type!=-1) { break; }
                }
                
            }
            randomType = Random.Range(5, 7);
            gembuffer.ChangeType(randomType,gemsSprites[randomType]);
        }
        numberSpecGemToCreate = 0;
    }
 
    public IEnumerator  Clear()
    {
        yield return new WaitForSeconds(0.1f);
        ClearMatches();
        turnHelper.ChekForPossibleTurn();
    }
   
    public void RandomColor()
    {
        randomType = Random.Range(0, level.colors);
    }
}

