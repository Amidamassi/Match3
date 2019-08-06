using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
[CustomEditor(typeof(Levels))]
public class LookAtPointEditor : Editor
{
    int levelSelect;
    int selectedLevel;
    Level level = new Level();
    Levels levels = new Levels();
    int[] numberArray = new int[] { 0};
    List<string> levelsNames= new List<string>();
    string[] namesArray;
    string[] sceneNames = new string[] {"Сцена 14*6, на счёт", "Сцена 14*6, на время"};
    int[] scenes = new int[] {0,1};
    GUIStyle[] styles=new GUIStyle[2];
    int[] boardTilesTypes =new int[] {0,1} ;
    string[] boardTilesTypeNames = new string[] { "Обычная", "Пустая"};
    int[,] boardSizes = new int[,] { { 14, 6 }, { 14, 6 } };
    private void OnEnable()
    {

        namesArray = levelsNames.ToArray();
        XmlSerializer deserializer = new XmlSerializer(typeof(string[]));
        StreamReader writer = new StreamReader(Application.dataPath + "/XML/Levels.xml");
        Debug.Log("ok");
        namesArray = (string[]) deserializer.Deserialize(writer);
        numberArray = new int[namesArray.Length];
        for(int i = 0; i < namesArray.Length; i++)
        {
            numberArray[i] = i;
            levelsNames.Add(namesArray[i]);
        }
        Debug.Log("ok2");
        writer.Dispose();
        
    }

    public override void OnInspectorGUI()
    {
        GUI.skin.label.normal.textColor = Color.black;
        GUI.skin.button.normal.textColor = Color.black;
        styles[0] = new GUIStyle("Button");
        styles[0].normal.textColor = Color.black;
        styles[1] = new GUIStyle("Button");
        styles[1].normal.textColor = Color.red;

        levelSelect = EditorGUILayout.IntPopup(levelSelect, namesArray, numberArray);
        if (GUILayout.Button("Загрузить"))
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Level));
            StreamReader writer = new StreamReader(Application.dataPath + "/XML/Level"+levelSelect +".xml");
            Debug.Log("ok");
            level =(Level) deserializer.Deserialize (writer);
            Debug.Log("ok2");
            writer.Dispose();
            selectedLevel = levelSelect;

        }

        EditorGUILayout.LabelField("Уровень " + selectedLevel);
        level.colors = EditorGUILayout.IntField("Количество цветов", level.colors);
        level.basikLevelScene = EditorGUILayout.IntPopup(level.basikLevelScene, sceneNames, scenes);
        level.levelNumber = selectedLevel;
        level.scoreLevel = EditorGUILayout.Toggle("Уровень на счёт?",level.scoreLevel);
        if (level.scoreLevel)
        {
            level.targetScore = EditorGUILayout.IntField("Кол-во очков", level.targetScore);
            level.targetTurns = EditorGUILayout.IntField("Кол-во ходов", level.targetTurns);
        }
        else
        {
            level.targetScore = EditorGUILayout.IntField("Кол-во очков", level.targetScore);
            level.targetTime = EditorGUILayout.IntField("Время на уровень", level.targetTime);
        }
        
        if ((level.boardHeight != boardSizes[level.basikLevelScene, 0]) |
                level.boardWidth != boardSizes[level.basikLevelScene, 1])
        {
            level.boardHeight = boardSizes[level.basikLevelScene, 0];
            level.boardWidth = boardSizes[level.basikLevelScene, 1];
            level.board = new int[level.boardHeight * level.boardWidth];
        }
        
        EditorGUILayout.BeginHorizontal();
        for (int k = 0; k < level.boardWidth; k++)
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < level.boardHeight; i++)
            {
                level.board[i + k * level.boardHeight] =
                    EditorGUILayout.IntPopup(level.board[i + k * level.boardHeight],
                                                boardTilesTypeNames,
                                                boardTilesTypes,
                                                styles[level.board[i + k * level.boardHeight]]);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Записать"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            StreamWriter writer = new StreamWriter(Application.dataPath + "/XML/Level" + levelSelect + ".xml");
            Debug.Log("ok");
            serializer.Serialize(writer, level);
            Debug.Log("ok2");
            writer.Dispose();

        }

        if (GUILayout.Button("Добавить уровень"))
        {
            
            levelSelect = namesArray.Length;
            levelsNames.Add("level" + levelSelect);
            namesArray = levelsNames.ToArray();
            XmlSerializer serializer = new XmlSerializer(typeof(string[]));
            StreamWriter writer = new StreamWriter(Application.dataPath + "/XML/Levels.xml");
            serializer.Serialize(writer, namesArray);
            writer.Dispose();
            numberArray = new int[namesArray.Length];
            for (int i = 0; i < namesArray.Length; i++)
            {
                numberArray[i] = i;
            }
            serializer = new XmlSerializer(typeof(Level));
            writer = new StreamWriter(Application.dataPath + "/XML/Level" + levelSelect + ".xml");
            Debug.Log("ok");
            serializer.Serialize(writer, level);
            Debug.Log("ok2");
            writer.Dispose();

        }

    }
    
}
