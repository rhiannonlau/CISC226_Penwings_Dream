using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Unity.Collections;

public class Options : MonoBehaviour
{
    private GameObject selected, lastSelected;

    private GameObject btnSound, btnControls, btnReturn;
    private GameObject soundCloche, controlsCloche, returnCloche;

    private bool onPnlSound, onPnlControls;
    private GameObject pnlSound, pnlControls;
    private GameObject pnlDefaultTooltip, pnlSoundTooltip, pnlControlsTooltip, pnlChangingControlsTooltip;

    // sound panel vars
    // private float masterVol
    private GameObject sldMasterVol, sldMusicVol, sldEffectsVol, btnResetSound;
    private TMP_Text txtMasterVol, txtMusicVol, txtEffectsVol;
    private GameObject resetSoundCloche;
    private GameObject activeSlider;
    private TMP_Text activeSliderText;
    private bool spedUp;
    private float? startHold = null; // a float that can be null
    [SerializeField] private float speedUpAfter = 1f;
    [SerializeField] private float incrementVal = 0.01f, increaseFactor = 50f;
    
    // controls panel vars
    private GameObject btnMoveLeft, btnMoveRight, btnJump, btnInteract, btnDuck, btnEleUp, btnEleDown, btnPause, btnResetControls;
    private TMP_Text txtMoveLeft, txtMoveRight, txtJump, txtInteract, txtDuck, txtEleUp, txtEleDown, txtPause;
    private TMP_Text txtCurrentControl, txtLastControl;
    private GameObject resetControlsCloche;
    private TMP_Text txtSelectedControl;
    private string playerPrefsKey;
    private GameObject btnCurrentControl;
    private bool growing;
    private float fontSize;


    private UISoundManager uiSoundManager;

    public void Awake()
    {
        // btnSound = transform.GetChild(0).GetChild(1).gameObject;
        // btnControls = transform.GetChild(0).GetChild(2).gameObject;
        // btnReturn = transform.GetChild(0).GetChild(3).gameObject;

        // soundCloche = btnSound.transform.GetChild(1).gameObject;
        // controlsCloche = btnControls.transform.GetChild(1).gameObject;
        // returnCloche = btnReturn.transform.GetChild(1).gameObject;

        // lastSelected = btnSound;

        // pnlSound = transform.GetChild(1).gameObject;
        // pnlControls = transform.GetChild(2).gameObject;
        // pnlDefaultTooltip = transform.GetChild(3).gameObject;
        // pnlSoundTooltip = transform.GetChild(4).gameObject;
        // pnlControlsTooltip = transform.GetChild(5).gameObject;
        // pnlChangingControlsTooltip = transform.GetChild(6).gameObject;

        // sldMasterVol = pnlSound.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        // sldMusicVol = pnlSound.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        // sldEffectsVol = pnlSound.transform.GetChild(3).GetChild(0).GetComponent<Slider>();

        // txtMasterVol = pnlSound.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        // txtMusicVol = pnlSound.transform.GetChild(2).GetChild(2).GetComponent<TMP_Text>();
        // txtEffectsVol = pnlSound.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>();
    }

    public void Start()
    {
        // get references to options panel objects
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

        // get references to sound panel objects
        sldMasterVol = pnlSound.transform.GetChild(1).GetChild(0).gameObject;
        sldMusicVol = pnlSound.transform.GetChild(2).GetChild(0).gameObject;
        sldEffectsVol = pnlSound.transform.GetChild(3).GetChild(0).gameObject;

        txtMasterVol = pnlSound.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        txtMusicVol = pnlSound.transform.GetChild(2).GetChild(2).GetComponent<TMP_Text>();
        txtEffectsVol = pnlSound.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>();

        // get the current sound settings from playerprefs with default from staticdata
        UpdateSoundDisplay();

        // btnResetSound = 
        // resetSoundCloche = 

        // get references to control panel objects
        btnMoveLeft = pnlControls.transform.GetChild(1).GetChild(2).gameObject;
        btnMoveRight = pnlControls.transform.GetChild(2).GetChild(2).gameObject;
        btnJump = pnlControls.transform.GetChild(3).GetChild(2).gameObject;
        btnInteract = pnlControls.transform.GetChild(4).GetChild(2).gameObject;
        btnDuck = pnlControls.transform.GetChild(5).GetChild(2).gameObject;
        btnEleUp = pnlControls.transform.GetChild(6).GetChild(2).gameObject;
        btnEleDown = pnlControls.transform.GetChild(7).GetChild(2).gameObject;
        btnPause = pnlControls.transform.GetChild(8).GetChild(2).gameObject;
        btnResetControls = pnlControls.transform.GetChild(9).gameObject;

        txtMoveLeft = btnMoveLeft.transform.GetChild(0).GetComponent<TMP_Text>();
        txtMoveRight = btnMoveRight.transform.GetChild(0).GetComponent<TMP_Text>();
        txtJump = btnJump.transform.GetChild(0).GetComponent<TMP_Text>();
        txtInteract = btnInteract.transform.GetChild(0).GetComponent<TMP_Text>();
        txtDuck = btnDuck.transform.GetChild(0).GetComponent<TMP_Text>();
        txtEleUp = btnEleUp.transform.GetChild(0).GetComponent<TMP_Text>();
        txtEleDown = btnEleDown.transform.GetChild(0).GetComponent<TMP_Text>();
        txtPause = btnPause.transform.GetChild(0).GetComponent<TMP_Text>();

        // get the current control settings from playerprefs with default from staticdata
        UpdateControlsDisplay();

        resetControlsCloche = btnResetControls.transform.GetChild(1).gameObject;

        AllOptionsFalse();
        onPnlSound = false;
        onPnlControls = false;

        AllPanelsFalse();

        // default first selection = sound menu
        EventSystem.current.SetSelectedGameObject(btnSound);

        uiSoundManager = GameObject.Find("UI Sound Manager").GetComponent<UISoundManager>();
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

        if (!onPnlSound && !onPnlControls)
        {
            AllTooltipsFalse();
            pnlDefaultTooltip.SetActive(true);

            // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
            if (!(selected == btnSound || selected == btnControls || selected == btnReturn))
            {
                EventSystem.current.SetSelectedGameObject(btnSound);
                selected = EventSystem.current.currentSelectedGameObject;
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
                    AllPanelsFalse();
                    onPnlSound = true;
                    pnlSound.SetActive(true);
                }

                else if(selected == btnControls)
                {
                    AllPanelsFalse();
                    onPnlControls = true;
                    pnlControls.SetActive(true);
                }

                else if(selected == btnReturn)
                {
                    Return();
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Return();
            }
        }

        else if (onPnlSound && !onPnlControls)
        {
            AllTooltipsFalse();
            pnlSoundTooltip.SetActive(true);

            // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
            if (!(selected == sldMasterVol || selected == sldMusicVol || selected == sldEffectsVol))
            {
                EventSystem.current.SetSelectedGameObject(sldMasterVol);
            }

            // depending on which option is currently being hovered, show the cloche
            if (selected == sldMasterVol)
            {
                activeSlider = sldMasterVol;
                activeSliderText = txtMasterVol;
            }

            else if (selected == sldMusicVol)
            {
                activeSlider = sldMusicVol;
                activeSliderText = txtMusicVol;
            }

            else if (selected == sldEffectsVol)
            {
                activeSlider = sldEffectsVol;
                activeSliderText = txtEffectsVol;
            }

            else if (selected == btnResetSound)
            {
                activeSlider = null;

                PlayerPrefs.SetFloat("VolumeMaster", StaticData.defMasterVol);
                PlayerPrefs.SetFloat("VolumeMusic", StaticData.defMusicVol);
                PlayerPrefs.SetFloat("VolumeEffects", StaticData.defEffectsVol);

                UpdateSoundDisplay();
            }

            else
            {
                activeSlider = null;
            }

            // track the time when the arrow keys were held down
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                startHold = Time.time;
            }

            // while the arrow keys are held down or pressed, increment the slider
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                activeSlider.GetComponent<Slider>().value -= incrementVal;
                activeSliderText.text = activeSlider.GetComponent<Slider>().value.ToString();
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                activeSlider.GetComponent<Slider>().value += incrementVal;
                activeSliderText.text = activeSlider.GetComponent<Slider>().value.ToString();
            }

            // update the static variables and actual volume in real time
            if (activeSlider == sldMasterVol)
            {
                // save the master value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeMaster", sldMasterVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateMasterVolume();
            }

            else if (activeSlider == sldMusicVol)
            {
                // save the music value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeMusic", sldMusicVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateMusicVolume();
            }

            else if (activeSlider == sldEffectsVol)
            {
                // save the effects value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeEffects", sldEffectsVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateEffectsVolume();
            }

            // when the arrow keys are released, stop tracking the time
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                startHold = null;
            }

            // if the increment has not been sped up and one of the arrows is being held down,
            // speed up the increase after a period of time
            if (!spedUp && startHold != null && Time.time - startHold >= speedUpAfter)
            {
                incrementVal *= increaseFactor;
                spedUp = true;
            }

            // if the increment has been sped up and time is not being tracked,
            // then the key has been released so we can return the increment to its
            // original speed
            if (spedUp && startHold == null)
            {
                incrementVal /= increaseFactor;
                spedUp = false;
            }
        }

        else if (!onPnlSound && onPnlControls)
        {
            AllTooltipsFalse();
            pnlControlsTooltip.SetActive(true);

            // if selected is not = to a button in this panel and lastSelected is not null, reset to btnStart
            if (!selected && (lastSelected == btnMoveLeft || lastSelected == btnMoveRight || lastSelected == btnJump || lastSelected == btnInteract || lastSelected == btnDuck || lastSelected == btnEleUp || lastSelected == btnEleDown || lastSelected == btnPause || lastSelected == btnResetControls))
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }

            else if (!(selected == btnMoveLeft || selected == btnMoveRight || selected == btnJump || selected == btnInteract || selected == btnDuck || selected == btnEleUp || selected == btnEleDown || selected == btnPause || selected == btnResetControls))
            {
                EventSystem.current.SetSelectedGameObject(btnMoveLeft);
                lastSelected = btnMoveLeft;
                // txtLastControl = btnMoveLeft.transform.GetChild(0).GetComponent<TMP_Text>();
            }

            // Debug.Log(selected);

            // get the key value corresponding to the control that is being hovered
            // so that it can be properly changed/saved in playerprefs
            if (selected == btnMoveLeft)
            {
                playerPrefsKey = "KeyMoveLeft";
            }

            else if (selected == btnMoveRight)
            {
                playerPrefsKey = "KeyMoveRight";
            }

            else if (selected == btnJump)
            {
                playerPrefsKey = "KeyJump";
            }

            else if (selected == btnInteract)
            {
                playerPrefsKey = "KeyInteract";
            }

            else if (selected == btnDuck)
            {
                playerPrefsKey = "KeyDuck";
            }

            else if (selected == btnEleUp)
            {
                playerPrefsKey = "KeyEleUp";
            }

            else if (selected == btnEleDown)
            {
                playerPrefsKey = "KeyEleDown";
            }

            else if (selected == btnPause)
            {
                playerPrefsKey = "KeyPause";
            }

            else if (selected == btnResetControls)
            {
                playerPrefsKey = null;
                // turn on the cloche
                resetControlsCloche.SetActive(true);
            }

            else
            {
                playerPrefsKey = null;
            }

            // set the reset cloche to false if reset isn't being hovered/selected
            if (selected != btnResetControls)
            {
                resetControlsCloche.SetActive(false);
            }

            // depending on which option is currently being hovered, increase the font size
            if (selected != btnControls && selected != btnSound)
            {
                txtSelectedControl = selected.transform.GetChild(0).GetComponent<TMP_Text>();
                txtSelectedControl.fontSize = 60;
            }

            if (lastSelected != selected && lastSelected != btnControls && lastSelected != btnSound)
            {
                TMP_Text txtLastSelectedControl = lastSelected.transform.GetChild(0).GetComponent<TMP_Text>();
                txtLastSelectedControl.fontSize = 50;
                
                if (txtLastSelectedControl.text == "")
                {
                    // reset to what it was saved as before
                    txtLastSelectedControl.text = PlayerPrefs.GetString(playerPrefsKey);

                    AllTooltipsFalse();
                    pnlControlsTooltip.SetActive(true);
                }
            }

            // the user has selected a key to change
            if (Input.GetKeyDown(KeyCode.X) && txtSelectedControl && playerPrefsKey != null)
            {
                AllTooltipsFalse();
                pnlChangingControlsTooltip.SetActive(true);

                txtSelectedControl.text = ""; // make the key text blank

                // bug status: when the user presses x, it registers
                // and sets the text to "" then instantly on the same loop takes that
                // x and enters it as the input for the new key
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keycode))
                    {
                        string key = keycode.ToString();
                        txtSelectedControl.text = key; // show the value that they've just inputted
                        PlayerPrefs.SetString(playerPrefsKey, keycode.ToString());
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.X) && selected == btnResetControls)
            {
                PlayerPrefs.SetString("KeyMoveLeft", StaticData.defMoveLeft.ToString());
                PlayerPrefs.SetString("KeyMoveRight", StaticData.defMoveRight.ToString());
                PlayerPrefs.SetString("KeyJump", StaticData.defJump.ToString());
                PlayerPrefs.SetString("KeyInteract", StaticData.defInteract.ToString());
                PlayerPrefs.SetString("KeyDuck", StaticData.defDuck.ToString());
                PlayerPrefs.SetString("KeyElevatorUp", StaticData.defEleUp.ToString());
                PlayerPrefs.SetString("KeyElevatorDown", StaticData.defEleDown.ToString());
                PlayerPrefs.SetString("KeyPause", StaticData.defPause.ToString());

                UpdateControlsDisplay();
            }

            // selected = EventSystem.current.currentSelectedGameObject;
            // txtCurrentControl = selected.transform.GetChild(0).GetComponent<TMP_Text>();

            // if (!txtLastControl)
            // {
            //     txtLastControl = selected.transform.GetChild(0).GetComponent<TMP_Text>();
            // }

            // if (txtLastControl != txtCurrentControl)
            // {
            //     txtLastControl.fontSize = 50;
            //     growing = true;
            // }

            // if (growing && fontSize >= 50 && fontSize < 55)
            // {
            //     fontSize += 1;

            //     if (fontSize == 55)
            //     {
            //         growing = false;
            //     }
            // }

            // else if (!growing && fontSize > 51 && fontSize <= 55)
            // {
            //     fontSize -= 1;

            //     if (fontSize == 50)
            //     {
            //         growing = true;
            //     }
            // }

            // txtCurrentControl.fontSize = fontSize;

            // txtLastControl = txtCurrentControl;

            lastSelected = selected;
        }

        if (onPnlSound || onPnlControls)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                onPnlSound = false;
                onPnlControls = false;
                AllPanelsFalse();

                // by default, playerprefs only saves to disk on game quit
                PlayerPrefs.Save(); // save all modifications to disk instantly
            }
        }

        // next: need if statements around most of the update to check
        // which panel we are currently on so that we can do two things:
        // 1. set the correct tooltip
        // 2. tell the game how to navigate between selections
    }

    private void Return()
    {
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("Options");
        }
    }

    private void AllOptionsFalse()
    {
        soundCloche.SetActive(false);
        controlsCloche.SetActive(false);
        returnCloche.SetActive(false);
    }

    private void AllPanelsFalse()
    {
        pnlSound.SetActive(false);
        pnlControls.SetActive(false);
    }

    private void AllTooltipsFalse()
    {
        pnlDefaultTooltip.SetActive(false);
        pnlSoundTooltip.SetActive(false);
        pnlControlsTooltip.SetActive(false);
        pnlChangingControlsTooltip.SetActive(false);
    }

    private void UpdateSoundDisplay()
    {
        float masterVol = PlayerPrefs.GetFloat("VolumeMaster", StaticData.defMasterVol) * 100;
        float musicVol = PlayerPrefs.GetFloat("VolumeMusic", StaticData.defMusicVol) * 100;
        float effectsVol = PlayerPrefs.GetFloat("VolumeEffects", StaticData.defEffectsVol) * 100;

        // PlayerPrefs.DeleteKey("MasterVolume");
        // PlayerPrefs.DeleteKey("MusicVolume");
        // PlayerPrefs.DeleteKey("EffectsVolume");
        // PlayerPrefs.Save();

        sldMasterVol.GetComponent<Slider>().value = masterVol;
        sldMusicVol.GetComponent<Slider>().value = musicVol;
        sldEffectsVol.GetComponent<Slider>().value = effectsVol;

        txtMasterVol.text = masterVol.ToString();
        txtMusicVol.text = musicVol.ToString();
        txtEffectsVol.text = effectsVol.ToString();
    }

    private void UpdateControlsDisplay()
    {
        txtMoveLeft.text = PlayerPrefs.GetString("KeyMoveLeft", StaticData.defMoveLeft.ToString());
        txtMoveRight.text = PlayerPrefs.GetString("KeyMoveRight", StaticData.defMoveRight.ToString());
        txtJump.text = PlayerPrefs.GetString("KeyJump", StaticData.defJump.ToString());
        txtInteract.text = PlayerPrefs.GetString("KeyInteract", StaticData.defInteract.ToString());
        txtDuck.text = PlayerPrefs.GetString("KeyDuck", StaticData.defDuck.ToString());
        txtEleUp.text = PlayerPrefs.GetString("KeyElevatorUp", StaticData.defEleUp.ToString());
        txtEleDown.text = PlayerPrefs.GetString("KeyElevatorDown", StaticData.defEleDown.ToString());
        txtPause.text = PlayerPrefs.GetString("KeyPause", StaticData.defPause.ToString());
    }
}
