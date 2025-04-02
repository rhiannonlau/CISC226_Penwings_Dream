using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    // [SerializeField] private Scene nextScene;

    [SerializeField] private RectTransform sprite;

    public void Start()
    {
        // LoadLevel(nextScene.name);
        // LoadLevel("Scene");
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }

    // private float delay = 2f;
    // private bool firstDelay;
    // private bool secondDelay;
    // private float random;

    // private float speed = 1;
    // float pos = 0;

    // public void Start()
    // {
        
    // }

    // public void Update()
    // {
    //     pos += speed * Time.deltaTime;
    //     slider.value = Mathf.Lerp(Time.time, slider.maxValue);
    // }

    // private IEnumerator fillBar(string x)
    // {
    //     yield return new WaitForSeconds(0.5f);
    // }
}