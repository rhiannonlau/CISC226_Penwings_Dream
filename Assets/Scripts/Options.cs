using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour
{
    private GameObject selected, lastSelected;

    private GameObject btnSound, btnControls, btnReturn;
    private GameObject soundCloche, controlsCloche, returnCloche;
    private GameObject activeCloche;

    private bool onPnlSound, onPnlControls;
    private GameObject pnlSound, pnlControls;
    private GameObject pnlDefaultTooltip, pnlSoundTooltip, pnlControlsTooltip, pnlChangingControlsTooltip;

    public void Awake()
    {
        btnSound = transform.GetChild(0).GetChild(1).gameObject;
        btnControls = transform.GetChild(0).GetChild(2).gameObject;
        btnReturn = transform.GetChild(0).GetChild(3).gameObject;

        soundCloche = btnSound.transform.GetChild(1).gameObject;
        controlsCloche = btnControls.transform.GetChild(1).gameObject;
        returnCloche = btnReturn.transform.GetChild(1).gameObject;

        lastSelected = btnSound;

        pnlSound = transform.GetChild(1).gameObject;
        pnlControls = transform.GetChild(2).gameObject;
        pnlDefaultTooltip = transform.GetChild(3).gameObject;
        pnlSoundTooltip = transform.GetChild(4).gameObject;
        pnlControlsTooltip = transform.GetChild(5).gameObject;
        pnlChangingControlsTooltip = transform.GetChild(6).gameObject;
    }

    public void Start()
    {
        AllOptionsFalse();
        onPnlSound = false;
        onPnlControls = false;

        // default first selection = sound menu
        EventSystem.current.SetSelectedGameObject(btnSound);        
    }

    public void Update()
    {
        // get the current selection
        selected = EventSystem.current.currentSelectedGameObject;

        // to catch edge cases where mouse deselects all options
        // reset the selected option to be the last known selection
        if (!selected)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }

        // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
        else if (!(selected == btnSound || selected == btnControls || selected == btnReturn) && !lastSelected)
        {
            EventSystem.current.SetSelectedGameObject(btnSound);
        }

        // depending on which option is currently being hovered, show the cloche
        if (selected == btnSound)
        {
            AllOptionsFalse();
            soundCloche.SetActive(true);
        }

        else if(selected == btnControls)
        {
            AllOptionsFalse();
            controlsCloche.SetActive(true);
        }

        else if(selected == btnReturn)
        {
            AllOptionsFalse();
            returnCloche.SetActive(true);
        }

        else
        {
            AllOptionsFalse();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (selected == btnSound)
            {
                AllOptionsFalse();
                soundCloche.SetActive(true);
            }

            else if(selected == btnControls)
            {
                AllOptionsFalse();
                controlsCloche.SetActive(true);
            }

            else if(selected == btnReturn)
            {
                AllOptionsFalse();
                returnCloche.SetActive(true);
            }
        }

        // next: need if statements around most of the update to check
        // which panel we are currently on so that we can do two things:
        // 1. set the correct tooltip
        // 2. tell the game how to navigate between selections
    }

    private void AllOptionsFalse()
    {
        soundCloche.SetActive(false);
        controlsCloche.SetActive(false);
        returnCloche.SetActive(false);
    }
}
