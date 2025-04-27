using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Unity.Collections;
using System.Text.RegularExpressions;

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
    private GameObject resetControlsCloche;
    private TMP_Text txtSelectedControl;
    private GameObject pnlNewControlPopUp;
    private TMP_Text txtControlToChange;
    private bool showingPopUp, waitingForKey;
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

        btnResetSound = pnlSound.transform.GetChild(4).gameObject;
        resetSoundCloche = btnResetSound.transform.GetChild(1).gameObject;

        // get references to control panel objects
        btnMoveLeft = pnlControls.transform.GetChild(2).GetChild(2).gameObject;
        btnMoveRight = pnlControls.transform.GetChild(3).GetChild(2).gameObject;
        btnEleUp = pnlControls.transform.GetChild(4).GetChild(2).gameObject;
        btnEleDown = pnlControls.transform.GetChild(5).GetChild(2).gameObject;
        btnJump = pnlControls.transform.GetChild(6).GetChild(2).gameObject;
        btnInteract = pnlControls.transform.GetChild(7).GetChild(2).gameObject;
        btnDuck = pnlControls.transform.GetChild(8).GetChild(2).gameObject;
        btnPause = pnlControls.transform.GetChild(9).GetChild(2).gameObject;
        btnResetControls = pnlControls.transform.GetChild(10).gameObject;

        txtMoveLeft = btnMoveLeft.transform.GetChild(0).GetComponent<TMP_Text>();
        txtMoveRight = btnMoveRight.transform.GetChild(0).GetComponent<TMP_Text>();
        txtEleUp = btnEleUp.transform.GetChild(0).GetComponent<TMP_Text>();
        txtEleDown = btnEleDown.transform.GetChild(0).GetComponent<TMP_Text>();
        txtJump = btnJump.transform.GetChild(0).GetComponent<TMP_Text>();
        txtInteract = btnInteract.transform.GetChild(0).GetComponent<TMP_Text>();
        txtDuck = btnDuck.transform.GetChild(0).GetComponent<TMP_Text>();
        txtPause = btnPause.transform.GetChild(0).GetComponent<TMP_Text>();

        pnlNewControlPopUp = pnlControls.transform.GetChild(11).gameObject;
        txtControlToChange = pnlNewControlPopUp.transform.GetChild(1).GetComponent<TMP_Text>();
        showingPopUp = false;

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
            if (!(selected == sldMasterVol || selected == sldMusicVol || selected == sldEffectsVol || selected == btnResetSound))
            {
                EventSystem.current.SetSelectedGameObject(sldMasterVol);
            }

            // depending on which option is currently being hovered, show the cloche
            if (selected == sldMasterVol)
            {
                activeSlider = sldMasterVol;
                activeSliderText = txtMasterVol;

                // save the master value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeMaster", sldMasterVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateMasterVolume();
            }

            else if (selected == sldMusicVol)
            {
                activeSlider = sldMusicVol;
                activeSliderText = txtMusicVol;

                // save the music value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeMusic", sldMusicVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateMusicVolume();
            }

            else if (selected == sldEffectsVol)
            {
                activeSlider = sldEffectsVol;
                activeSliderText = txtEffectsVol;

                // save the effects value to playerprefs and update in real time
                PlayerPrefs.SetFloat("VolumeEffects", sldEffectsVol.GetComponent<Slider>().value / 100);
                uiSoundManager.UpdateEffectsVolume();
            }

            // if hovering the reset button
            else if (selected == btnResetSound)
            {
                activeSlider = null;

                // turn on cloche
                resetSoundCloche.SetActive(true);

                // if reset button pressed
                if (Input.GetKeyDown(KeyCode.X))
                {
                    PlayerPrefs.SetFloat("VolumeMaster", StaticData.defMasterVol);
                    PlayerPrefs.SetFloat("VolumeMusic", StaticData.defMusicVol);
                    PlayerPrefs.SetFloat("VolumeEffects", StaticData.defEffectsVol);

                    UpdateSoundDisplay();

                    uiSoundManager.UpdateMasterVolume();
                }
            }

            else
            {
                activeSlider = null;
            }

            // turn off the cloche if not hovering the reset button
            if (selected != btnResetSound)
            {
                resetSoundCloche.SetActive(false);
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
                playerPrefsKey = "KeyElevatorUp";
            }

            else if (selected == btnEleDown)
            {
                playerPrefsKey = "KeyElevatorDown";
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
            if (selected != btnControls && selected != btnSound && selected != btnReturn && selected != btnResetControls)
            {
                txtSelectedControl = selected.transform.GetChild(0).GetComponent<TMP_Text>();
                txtSelectedControl.fontSize = 50;
                txtSelectedControl.color = Color.red;
            }

            if (lastSelected != selected && lastSelected != btnControls && lastSelected != btnSound && lastSelected != btnReturn && lastSelected != btnResetControls)
            {
                TMP_Text txtLastSelectedControl = lastSelected.transform.GetChild(0).GetComponent<TMP_Text>();
                txtLastSelectedControl.fontSize = 45;
                txtLastSelectedControl.color = Color.white;
                // StartCoroutine(Pulse(txtSelectedControl));
            }

            pnlNewControlPopUp.SetActive(showingPopUp);

            // the user has selected a key to change
            if (Input.GetKeyDown(KeyCode.X) && txtSelectedControl && playerPrefsKey != null && !showingPopUp)
            {
                showingPopUp = true;
                waitingForKey = false; // reset waiting flag

                var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
                txtControlToChange.text = r.Replace(playerPrefsKey.Substring(3), " ");  // display the action they are rebinding using regex for spacing
            }

            else if (Input.GetKeyDown(KeyCode.X) && selected == btnResetControls && !showingPopUp)
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

            // show the key change pop up
            if (showingPopUp)
            {
                if (!waitingForKey)
                {
                    // wait until the next frame
                    waitingForKey = true;
                    return;
                }

                // on the next frame, start detecting key input
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keycode))
                    {
                        string key = keycode.ToString();
                        txtSelectedControl.text = key; // show the value that they've just inputted
                        PlayerPrefs.SetString(playerPrefsKey, key);

                        showingPopUp = false;
                        waitingForKey = false;
                        break; // stop checking other keys
                    }
                }
            }

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
    }

    private void Return()
    {
        int n = SceneManager.sceneCount;

        if (n > 1)
        {
            SceneManager.UnloadSceneAsync("Options");
        }
    }

    IEnumerator Pulse(TMP_Text txt)
	{
        float approachSpeed = 0.02f;
        float growthBound = 5f;
        float shrinkBound = 0.5f;
        float currentRatio = 1;

		// Run this indefinitely
		while (lastSelected == selected)
		{
			// Get bigger for a few seconds
			while (currentRatio != growthBound)
			{
				// Determine the new ratio to use
				currentRatio = Mathf.MoveTowards( currentRatio, growthBound, approachSpeed);

				// Update our text element
				txt.transform.localScale = Vector3.one * currentRatio;
				txt.text = "Growing!";

				yield return new WaitForEndOfFrame();
			}

			// Shrink for a few seconds
			while (currentRatio != shrinkBound)
			{
				// Determine the new ratio to use
				currentRatio = Mathf.MoveTowards( currentRatio, shrinkBound, approachSpeed);

				// Update our text element
				txt.transform.localScale = Vector3.one * currentRatio;
				txt.text = "Shrinking!";

				yield return new WaitForEndOfFrame();
			}
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
