using UnityEngine;

public class Spear : MonoBehaviour
{
    public float speed = 20f;
    public float maxDistance = 8f;
    public float returnSpeed = 25f;

    private Vector3 startPosition;
    private Transform player;

    public bool IsReturning => returning;
    public bool IsStopped = false;

    private bool returning = false;

    public System.Action OnHarpoonReturned;

    public static Spear instance;

    void Start()
    {
        startPosition = transform.position;
        instance = this;
    }

    public void Setup(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Update()
    {
        if (!returning)
        {
            transform.position += transform.right * speed * Time.deltaTime;

            if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
            {
                IsStopped = true;

                Invoke(nameof(ReturnHarpoon), 0.3f);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                returnSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, player.position) < 0.2f)
            {
                OnHarpoonReturned?.Invoke();

                Destroy(gameObject);
            }
        }
    }

    void ReturnHarpoon()
    {
        returning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Invoke(nameof(ReturnHarpoon), 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ReturnHarpoon();
        }
    }
}