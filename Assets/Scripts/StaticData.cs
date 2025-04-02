using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static Dictionary<string, float> goals = new Dictionary<string, float>();
    public static Dictionary<string, float> scores = new Dictionary<string, float>();
    public static Dictionary<string, float> highestSatisfactions = new Dictionary<string, float>();

    public static float goal, score, highestSatisfaction;

    public static string justPlayed;

    public static bool toPostGame;
}
