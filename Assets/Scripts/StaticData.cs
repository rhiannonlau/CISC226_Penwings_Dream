using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class StaticData : MonoBehaviour
{
    // KEY BINDINGS INFORMATION /////////////////////////////////////
    // public static KeyCode moveLeft, moveRight;



    // SOUND INFORMATION /////////////////////////////////////////
    public static float masterVolume = 0.7f, musicVolume = 0.7f, effectsVolume = 0.7f;

    
    // GAME INFORMATION /////////////////////////////////////
    public static bool tutorialCompleted = false;

    // dictionaries for storing high scores
    public static Dictionary<string, float> goals = new Dictionary<string, float>();
    public static Dictionary<string, float> highscores = new Dictionary<string, float>();
    public static Dictionary<string, float> highestSatisfactions = new Dictionary<string, float>();

    // store the information from the game that was just played
    // to display in the post game screen
    public static float goal, score, highestSatisfaction;
    public static string currentLevel, justPlayed;

    // alert menus that we want to go to the post game screen, not the main menu
    // after the game
    public static bool toPostGame;
}
