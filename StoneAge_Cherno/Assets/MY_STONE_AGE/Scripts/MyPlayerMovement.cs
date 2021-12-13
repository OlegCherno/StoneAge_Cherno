using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class MyPlayerMovement : MonoBehaviour
{

    [Header("Player Property")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpForce;

    [SerializeField] private GameObject animObject;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeDelay;

    //private SpriteRenderer spriteRenderer;

    private float currentPlayerSpeed;
    private Rigidbody2D rb;
    private bool groundCheck;
    private int countJump;                                                                     // Для реализации двойного прыжка
    private Collider2D colliderWeapon;

    public event UnityAction<GameObject, Vector2, GameObject> NotifyPlayerCollision;           // Обявляем событие

    private void Awake()
    {
        colliderWeapon = transform.GetChild(0).GetChild(0).GetComponent<Collider2D>();
        colliderWeapon.enabled = false;
        colliderWeapon.isTrigger = false;
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = animObject.GetComponent<SpriteRenderer>();
        animator = animObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))           //    Вызов бега
        {
            Run();
            animator.SetTrigger("Run");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            StopMove();
    }
   
    private void FixedUpdate()
    {
                     
          //ДЗ добавить enum

        rb.velocity = new Vector2(currentPlayerSpeed * Time.fixedDeltaTime, rb.velocity.y);
        //rb.AddForce(new Vector2(currentPlayerSpeed, 0f), ForceMode2D.Force);

        animator.SetFloat("Speed", Mathf.Abs(currentPlayerSpeed));
    }
    //---------------------------Временная функция бега ------------------------------------
    private void Run()
    {
        float movement = Input.GetAxis("Horizontal");
        if (movement > 0)
        {
            currentPlayerSpeed = playerSpeed * 1.5f;
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        }
        else
        { 
                currentPlayerSpeed = -playerSpeed * 1.5f;
                if (transform.localScale.x > 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
        }

    }
    //---------------------------------------------------------------------------------------

    public void RightMove()
    {
        currentPlayerSpeed = playerSpeed;
        // spriteRenderer.flipX = false; 
        if (transform.localScale.x < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
    }
    public void LeftMove()
    {
        currentPlayerSpeed = -playerSpeed;
        //spriteRenderer.flipX = true;
        if (transform.localScale.x > 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
    }


    public void StopMove()
    {
        currentPlayerSpeed = 0f;
    }

    public void Jump()
    {
        if (groundCheck || countJump <=1 )
        {
            groundCheck = false;
            countJump++;
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            
        }
    }

    public void Attack()
    {
        colliderWeapon.enabled = true;
      //  colliderWeapon.isTrigger = false;
        animator.SetTrigger("Attack");
        StartCoroutine(WaitEnd(timeDelay));

    }

    IEnumerator WaitEnd(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        colliderWeapon.enabled = false;
      //  colliderWeapon.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        groundCheck = true;
        countJump = 0;
        GameObject other = collision.gameObject;
      //  if( !other.name.ToLower().Contains("precipice") )
            NotifyPlayerCollision?.Invoke(other, new Vector2(0f, 0f), gameObject);               // Вызываем событие
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        NotifyPlayerCollision?.Invoke(other, collision.contacts[0].point, gameObject);             // Вызываем событие
    }
}



//if (transform.localScale.x > 0)
//{
//    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
//}