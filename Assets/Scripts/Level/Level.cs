using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Level
{
    public int basikLevelScene;
    public int boardWidth;
    public int boardHeight;
    public int colors;
    public int levelNumber;
    public int[] board;
    public int targetScore;
    public int targetTime;
    public int targetTurns;
    public bool scoreLevel;
    public Level()
    {
    }
    
}