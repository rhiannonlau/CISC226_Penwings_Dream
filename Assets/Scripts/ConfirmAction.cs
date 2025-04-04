using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmAction : MonoBehaviour
{
    private TMP_Text txtMessage;
    
    private GameObject btnYes, btnNo;

    private GameObject yesCloche, noCloche;

    public void QuitGame()
    {

    }

    public void RestartLeve()
    {
        txtMessage.text = "Are you sure you want to restart? All your progress will be lost.";
    }

    public void QuitLevel()
    {
        txtMessage.text = "Are you sure you want to quit? All your progress will be lost.";
    }
}
