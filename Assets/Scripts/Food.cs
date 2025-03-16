using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Food : MonoBehaviour
{
    // components
    private Collider2D collideWithPlayer;
    private Collider2D ignorePlayer;
    private Collider2D playerColl;
    // private Rigidbody2D body;

    // properties
    public int id { get; private set; }
    
    // layer references
    private LayerMask tableLayer;
    private LayerMask counterLayer;
    private LayerMask foodLayer;

    // Start is called before the first frame update
    void Start()
    {
        collideWithPlayer = GetComponent<CircleCollider2D>();
        ignorePlayer = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onTableOrCounter())
        {
            // collideWithPlayer.isTrigger = true;
        }

        else
        {
            // collideWithPlayer.isTrigger = false;
        }
    }

    private bool onTableOrCounter()
    {
        Vector2 position = new Vector2(collideWithPlayer.bounds.center.x, collideWithPlayer.bounds.center.y - collideWithPlayer.bounds.size.y * 0.5f);
        Vector2 size = new Vector2(0.2f, collideWithPlayer.bounds.size.y * 0.9f);
        RaycastHit2D belowInfo = Physics2D.CapsuleCast(position, size, CapsuleDirection2D.Horizontal, 0, Vector2.down, 0.2f);

        LayerMask hitLayer = belowInfo.transform.gameObject.layer;
        return hitLayer == tableLayer || hitLayer == counterLayer;
    }
}
