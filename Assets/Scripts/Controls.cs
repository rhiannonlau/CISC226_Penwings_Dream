using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Back();
        }
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("StartMenu");
    }
}
