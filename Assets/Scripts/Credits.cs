using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // reference canvas to access MenuManager.cs to change panels
    [SerializeField] private Canvas canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Back();
        }
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
