using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
public class LevelDownloader : MonoBehaviour
{
    private Level currentLevel;
    private int currentLevelNumber;
    private void Start()
    {
        currentLevelNumber = PlayerPrefs.GetInt("currentLevel");
    }
    public void NextLevel()
    {
        currentLevelNumber++;
        XmlSerializer deserializer = new XmlSerializer(typeof(Level));
        TextAsset text = (Resources.Load("XML/Level" + currentLevelNumber) as TextAsset);
        StringReader stream = new StringReader(text.text);
        currentLevel = (Level)deserializer.Deserialize(stream);
        PlayerPrefs.SetInt("currentLevel", currentLevelNumber);
        stream.Dispose();
        SceneManager.LoadScene("BasicLevel"+currentLevel.basikLevelScene);
    }
    public void Restart()
    {
        SceneManager.LoadScene("BasicLevel" + currentLevel.basikLevelScene);
    }
    public void LoadLevel(int number)
    {
        XmlSerializer deserializer = new XmlSerializer(typeof(Level));
        TextAsset text = (Resources.Load("XML/Level" + number) as TextAsset );
        StringReader stream = new StringReader(text.text);
        currentLevel = (Level)deserializer.Deserialize(stream);
        PlayerPrefs.SetInt("currentLevel", number);
        stream.Dispose();
        SceneManager.LoadScene("BasicLevel" + currentLevel.basikLevelScene);
        
    }
}
