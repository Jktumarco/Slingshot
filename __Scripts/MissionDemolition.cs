using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShort;
    public Text uitButton;

    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMaX;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
   
    void Start()
    {
        S = this;

        level = 0;
        levelMaX = castles.Length;
        StartLevel();
    }
   
    void StartLevel()
    {
        
    
        if (castle != null) Destroy(castle);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        UpdateGUI();

        mode = GameMode.playing;


    }
    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMaX;
        uitShort.text = "Short Taken: "+shotsTaken;
    }
   
    void Update() { 
        UpdateGUI();
        
    
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 1f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMaX)
        {
            level = 0;
        }
        StartLevel();
        Vector3 cam = Camera.main.transform.position;
        cam = Vector3.one;
        
        transform.position = cam;
    }
        
    public void SwitchView( string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingchot";
                break;
        }
    }
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
