using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    // full video on how to make key bindings changeable
    // https://discussions.unity.com/t/how-do-i-allow-users-to-change-controls-new-input/867908/4

    // how to get the key being pressed
    // https://discussions.unity.com/t/how-do-i-set-a-variable-to-the-key-im-pressing/94529/2
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
