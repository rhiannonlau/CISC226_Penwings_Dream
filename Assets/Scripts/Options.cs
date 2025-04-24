using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private GameObject sldMasterVol, sldMusicVol, sldEffectsVol;
    private TMP_Text txtMasterVol, txtMusicVol, txtEffectsVol;
    private GameObject activeSlider;
    private TMP_Text activeSliderText;
    private bool spedUp;
    private float? startHold = null; // a float that can be null
    [SerializeField] private float speedUpAfter = 1f;
    [SerializeField] private float incrementVal = 0.01f, increaseFactor = 50f;
    public UISoundManager uiSoundManager;

    // controls panel vars

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

        sldMasterVol = pnlSound.transform.GetChild(1).GetChild(0).gameObject;
        sldMusicVol = pnlSound.transform.GetChild(2).GetChild(0).gameObject;
        sldEffectsVol = pnlSound.transform.GetChild(3).GetChild(0).gameObject;

        txtMasterVol = pnlSound.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
        txtMusicVol = pnlSound.transform.GetChild(2).GetChild(2).GetComponent<TMP_Text>();
        txtEffectsVol = pnlSound.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>();

        sldMasterVol.GetComponent<Slider>().value = StaticData.masterVolume * 100;
        sldMusicVol.GetComponent<Slider>().value = StaticData.musicVolume * 100;
        sldEffectsVol.GetComponent<Slider>().value = StaticData.effectsVolume * 100;

        txtMasterVol.text = (StaticData.masterVolume  * 100).ToString();
        txtMusicVol.text = (StaticData.musicVolume  * 100).ToString();
        txtEffectsVol.text = (StaticData.effectsVolume  * 100).ToString();

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

            else if(selected == sldMusicVol)
            {
                activeSlider = sldMusicVol;
                activeSliderText = txtMusicVol;
            }

            else if(selected == sldEffectsVol)
            {
                activeSlider = sldEffectsVol;
                activeSliderText = txtEffectsVol;
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
                StaticData.masterVolume = sldMasterVol.GetComponent<Slider>().value / 100;
                uiSoundManager.UpdateMasterVolume();
            }

            else if (activeSlider == sldMusicVol)
            {
                StaticData.musicVolume = sldMusicVol.GetComponent<Slider>().value / 100;
                uiSoundManager.UpdateMusicVolume();
            }

            else if (activeSlider == sldEffectsVol)
            {
                StaticData.effectsVolume = sldEffectsVol.GetComponent<Slider>().value / 100;
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

            // if selected is not = to a button in this panel and lastSelected is null, reset to btnStart
            if (!(selected == btnSound || selected == btnControls || selected == btnReturn) && !lastSelected)
            {
                EventSystem.current.SetSelectedGameObject(btnSound);
            }

            // depending on which option is currently being hovered, show the cloche
            // if (selected == btnSound)
            // {
            //     AllOptionsFalse();
            //     soundCloche.SetActive(true);
            // }

            // else if(selected == btnControls)
            // {
            //     AllOptionsFalse();
            //     controlsCloche.SetActive(true);
            // }

            // else if(selected == btnReturn)
            // {
            //     AllOptionsFalse();
            //     returnCloche.SetActive(true);
            // }

            // else
            // {
            //     AllOptionsFalse();
            // }

            // if (Input.GetKeyDown(KeyCode.X))
            // {
            //     if (selected == btnSound)
            //     {
            //         AllOptionsFalse();
            //         soundCloche.SetActive(true);
            //     }

            //     else if(selected == btnControls)
            //     {
            //         AllOptionsFalse();
            //         controlsCloche.SetActive(true);
            //     }

            //     else if(selected == btnReturn)
            //     {
            //         AllOptionsFalse();
            //         returnCloche.SetActive(true);
            //     }
            // }
        }

        if (onPnlSound || onPnlControls)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                onPnlSound = false;
                onPnlControls = false;
                AllPanelsFalse();
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
}
