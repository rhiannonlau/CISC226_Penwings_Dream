using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Back();
        }
    }

    public void Back()
    {
        Debug.Log("Going back");
        SceneManager.LoadSceneAsync("StartMenu");
    }
}
