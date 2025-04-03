using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
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


    // where the sprite starts and ends
    private float leftXBoundary = 125f, rightXBoundary = 840f;
    private float distance; // calculate the distance the sprite has to travel
    private float xPos = 0; // the sprite's x position

    private string nextScene, video, justPlayed;

    public void Awake()
    {
        // calculate distance as diff between start and end point
        distance = Mathf.Abs(rightXBoundary) - Mathf.Abs(leftXBoundary);
    }

    public void OnEnable()
    {
        progressBar.value = 0; // reset progress bar

        nextScene = "";
        video = "";
        justPlayed = "";

        Debug.Log("Is this GameObject active? " + gameObject.activeInHierarchy);
        StartCoroutine(FillBar());
    }

    public void Update()
    {
        // if the progress bar hasn't been filled (value = 1 means it's full)
        if (progressBar.value != 1)
        {
            // StartCoroutine(FillBar());

            if (Random.Range(0, 10) < 1)
            {
                // progress the bar
                progressBar.value += 0.005f;

                // move the sprite proportionally to the amount the bar has been filled
                xPos = leftXBoundary + progressBar.value * distance;
                sprite.position = new Vector2(xPos, sprite.position.y);
            }
        }

        if (progressBar.value == 1)
        {
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
                canvas.GetComponent<MenuManager>().ToVideo(video);
            }
        }
    }

    private IEnumerator FillBar()
    {
        // do a coinflip to add some occasional delay to the
        // filling of the bar so it isn't linear every time
        if (Random.Range(0, 2) < 1)
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
