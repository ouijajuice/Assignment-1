using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterFollow : MonoBehaviour
{
    public GameObject explosionParticles;
    public LayerMask enemyLayer;
    private Transform target;
    public float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            rb.position = Vector2.MoveTowards(rb.position, closestEnemy.position, speed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayer);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = collider.transform;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionParticles, rb.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

}
