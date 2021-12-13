using UnityEngine.Events;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public event UnityAction<GameObject, Vector2, GameObject> NotifyCollision;           // Обявляем событие

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        NotifyCollision?.Invoke(other, collision.contacts[0].point, gameObject);         // Вызываем событие
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        NotifyCollision?.Invoke(other, new Vector2(0f, 0f), gameObject);                   // Вызываем событие
    }
}