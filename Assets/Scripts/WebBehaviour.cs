using UnityEngine;

public class WebBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 0) // change to layer in the future for better practice
        {
            Debug.Log("hit web");
            other.gameObject.SendMessage("Slow");
        }
    }
}
