using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KokonutBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.name.ToLower().Contains("ground"))
            StartCoroutine(WaitNsec(0.7f));
    }

    IEnumerator WaitNsec(float n)
    {
        yield return new WaitForSeconds(n);
        rb.gravityScale = 0;
        rb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        col.isTrigger = true;      
    }
}
