using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    // reference canvas to access MenuManager.cs to change panels
    [SerializeField] private Canvas canvas;

    // progress bar
    [SerializeField] private Slider progressBar;

    // used to move penwing's sprite and follow bar
    [SerializeField] private RectTransform sprite;

    private SpriteRenderer spriteRend;

    private Animator anim;


    // where the sprite starts and ends
    private float leftXBoundary = -400f, rightXBoundary = 425f;
    private float y = 65f;
    private float distance; // calculate the distance the sprite has to travel
    private float xPos = 0; // the sprite's x position

    private string nextScene, video, justPlayed;

    public void Awake()
    {
        spriteRend = sprite.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // calculate distance as diff between start and end point
        distance = Mathf.Abs(rightXBoundary) + Mathf.Abs(leftXBoundary);
    }

    public void OnEnable()
    {
        Application.runInBackground = true; // keep the loading screen going if the user alt tabs

        sprite.localPosition = new Vector2(leftXBoundary, y);
        anim.SetBool("walk", true);
        // spriteRend.enabled = true;

        progressBar.value = 0; // reset progress bar

        nextScene = "";
        video = "";
        justPlayed = "";

        StartCoroutine(FillBar());
    }

    public void Update()
    {
        // if the progress bar hasn't been filled (value = 1 means it's full)
        if (progressBar.value != 1)
        {
            // StartCoroutine(FillBar());

            if (Random.Range(0, 3) < 1)
            {
                // progress the bar
                progressBar.value += 0.005f;

                // move the sprite proportionally to the amount the bar has been filled
                xPos = leftXBoundary + progressBar.value * distance;
                // Debug.Log(xPos);
                // Debug.Log(distance);
                sprite.localPosition = new Vector2(xPos, y);
            }
        }

        if (progressBar.value == 1)
        {
            // spriteRend.enabled = false;

            // run in background enables the game to play when the user alt tabs
            // turn this off after leaving the loading screen to prevent
            // the game from running in the bg
            Application.runInBackground = false;

            if (nextScene != "")
            {
                canvas.GetComponent<MenuManager>().FromLoadingToLevel(nextScene);
            }

            if (justPlayed != "")
            {
                canvas.GetComponent<MenuManager>().FromLoadingToPostGame();
            }

            if (video != "")
            {
                canvas.GetComponent<MenuManager>().FromLoadingToVideo(video);
            }
        }
    }

    private IEnumerator FillBar()
    {
        // do a coinflip to add some occasional delay to the
        // filling of the bar so it isn't linear every time
        if (Random.Range(0, 3) < 1)
        {
            yield return new WaitForSeconds(0.5f);
        }

        else
        {
            // progress the bar
            progressBar.value += 0.01f;

            // move the sprite proportionally to the amount the bar has been filled
            xPos = leftXBoundary + progressBar.value * distance;
            sprite.position = new Vector2(xPos, sprite.position.y);
        }
    }

    public void ToScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public void ToPostGame(string level)
    {
        justPlayed = level;
    }

    public void ToVideo(string video)
    {
        this.video = video;
    }

    // if needed in the future: code for the loading bar that actually follows
    // the loading process of the next scene
    // public void Start()
    // {
    //     // LoadLevel(nextScene.name);
    //     // LoadLevel("Scene");
    // }

    // public void LoadLevel(string sceneName)
    // {
    //     StartCoroutine(LoadAsync(sceneName));
    // }

    // private IEnumerator LoadAsync(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

    //     loadingScreen.SetActive(true);

    //     while (!operation.isDone)
    //     {
    //         float progress = Mathf.Clamp01(operation.progress / 0.9f);

    //         progressBar.value = progress;

    //         yield return null;
    //     }
    // }
}
