using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyInteractions : MonoBehaviour
{
    // to check if near
    [SerializeField] private Rigidbody2D rbplayer;
    [SerializeField] private Rigidbody2D rbNPC;
    public bool playerclose;
    public float dectectrange = 1.5f;

    // setup money
    public double total = 0.00;
    public TextMeshProUGUI counter;

    public TextMeshProUGUI timerUI;
    private float interTime;
    private float timeRemain;
    private bool npcInter = false;
    private Coroutine timerCoroutine;

    // time before NPC requests interaction
    [SerializeField] public float minWaitTime, maxWaitTime;

    // Time the player has to respond
    [SerializeField] public float minInterTime, maxInterTime;
    [SerializeField] private Canvas canvas;

    private float oneThird;


    // Start is called before the first frame update
    void Start()
    {
        counter.text = "$" + total.ToString("0.00");
        timerUI.gameObject.SetActive(false);
        StartCoroutine(NPCDecide());
    }

    // Update is called once per frame
    void Update()
    {

        touch();

        if(Input.GetKeyDown(KeyCode.W) && playerclose)
        {
            Interactions();
        }
        // makes timer follow NPC
        if (npcInter)
        {
            MoveTimerUI();
        }
    }

    IEnumerator NPCDecide()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // NPC now requires interaction
            npcInter = true;
            interTime = Random.Range(minInterTime, maxInterTime); // Time the player has to respond
            timeRemain = interTime;
            oneThird = interTime / 3f;

            // Start countdown
            timerUI.gameObject.SetActive(true);
            timerCoroutine = StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        while (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
            timerUI.text = Mathf.Ceil(timeRemain).ToString();
            yield return null;
        }

        // Time ran out, player loses chance to earn money
        timerUI.gameObject.SetActive(false);
        npcInter = false;
        timerUI.text = "";
    }

    void Interactions()
    {
        if (!npcInter) return;

        if (timeRemain > oneThird * 2)
        {
            total += 15; // Fast response
        }
        else if (timeRemain > oneThird)
        {
            total += 12; // Medium response
        }
        else
        {
            total += 10; // Too late, no tip
        }

        counter.text = "$" + total.ToString("0.00");

        // Reset NPC state
        npcInter = false;
        timerUI.gameObject.SetActive(false);
        timerUI.text = "";

        // Stop any ongoing timer
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        // Restart the NPC decision cycle
        StartCoroutine(NPCDecide());
    }

    void MoveTimerUI()
    {
        if (canvas == null) return;

        // Convert NPC world position to screen space
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rbNPC.position + new Vector2(0.2f, 2f));

        // Convert screen position to canvas local position
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPos);

        // Assign new position
        timerUI.rectTransform.localPosition = localPos;
    }

    void touch()
    {
        if (Vector2.Distance(rbplayer.position, rbNPC.position) <= dectectrange)
        {
            playerclose = true;
        }
        else
        {
            playerclose = false;
        }
    }

}
