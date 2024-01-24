using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform startPos;
    public LayerMask playerLayer;
    public Transform detectionPosition;
    private Transform target;
    public float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.transform;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(rb.position, target.position) < 7f)
        {
            rb.position = Vector2.MoveTowards(rb.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            rb.position = Vector2.MoveTowards(rb.position, startPos.position, speed * Time.deltaTime);
        }
        
    }
}
