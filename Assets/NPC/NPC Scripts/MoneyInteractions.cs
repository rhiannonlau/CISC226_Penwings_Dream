using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyInteractions : MonoBehaviour
{
    // to check if near
    [SerializeField] private Rigidbody2D rbplayer;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody2D rbNPC;
    [SerializeField] private NPCWandering wanderScript;
    public bool playerclose;
    public float dectectrange = 1.5f;

    //chair
    [SerializeField] private Transform chairpos;
    [SerializeField] private float sitting;

    // setup money
    public double total = 0.00;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI earned;
    private double money = 10.00;

    // timer setup
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
        earned.gameObject.SetActive(false);
        StartCoroutine(NPCDecide());
    }

    // Update is called once per frame
    void Update()
    {

        touch();

        if(Input.GetKeyDown(KeyCode.W) && playerclose && npcInter)
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

            wanderScript.StopWandering();
            yield return StartCoroutine(chair());

            // Freezes the NPC in place to sit
            rbNPC.velocity = Vector2.zero;
            rbNPC.bodyType = RigidbodyType2D.Static;

            ani.SetBool("iswalk", false);

            yield return new WaitForSeconds(2f);


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

    IEnumerator chair()
    {
        ani.SetBool("iswalk", true);
        while (Vector2.Distance(rbNPC.position, chairpos.position) > 0.1f)
        {
            rbNPC.position = Vector2.MoveTowards(rbNPC.position, chairpos.position, Time.deltaTime * 2);
            yield return null;
        }
        ani.SetBool("iswalk", false);
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
    }

    void Interactions()
    {
        if (!npcInter) return;

        // Reset NPC state
        npcInter = false;
        timerUI.gameObject.SetActive(false);


        // Stop any ongoing timer
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }



        // Restart the NPC decision cycle

        StartCoroutine(Givemoney());
    }

    IEnumerator Givemoney()
    {
        yield return new WaitForSeconds(sitting);

        double earnedmoney = 0;

        if (timeRemain > oneThird * 2)
        {
            earnedmoney = 5 + money;
        }
        else if (timeRemain > oneThird)
        {
            earnedmoney = 2 + money;
        }
        else
        {
            earnedmoney = money;
        }

        total += earnedmoney;

        // earned money text
        earned.text = "+$" + earnedmoney.ToString("0.00");
        earned.gameObject.SetActive(true);

        MoveEarned();

        // total money text
        counter.text = "$" + total.ToString("0.00");
        yield return new WaitForSeconds(1f);

        earned.gameObject.SetActive(false);
        yield return new WaitForSeconds(8f);

        // Unfreezes NPC movement
        rbNPC.bodyType = RigidbodyType2D.Dynamic;
        ani.SetBool("iswalk", true);

        wanderScript.StartWandering();

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

    void MoveEarned()
    {
        if (canvas == null) return;

        // Convert NPC world position to screen space
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rbNPC.position + new Vector2(0.2f, 2f));

        // Convert screen position to canvas local position
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPos);

        // Assign new position
        earned.rectTransform.localPosition = localPos;
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

