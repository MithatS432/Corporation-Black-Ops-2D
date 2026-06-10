using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 2f;

    private Vector2 dir;

    public void Init(Vector2 direction)
    {
        dir = direction;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Weapon"))
            return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.GetDamage(1);
        }

        Destroy(gameObject);
    }
}