using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish Info")]
    public FishData fishData;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Random")]
    public float minChangeTime = 0.5f;
    public float maxChangeTime = 2f;

    [Header("Swim Area")]
    public Collider2D waterArea;

    [Header("Fear")]
    public float detectDistance = 3f;
    public float panicSpeed = 5f;
    public float panicDuration = 2f;

    Vector2 moveDirection;

    bool isCaught = false;
    bool isPanicking = false;

    Rigidbody2D rb;
    Transform player;

    Inventory inventory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // GameObject playerObj = GameObject.FindGameObjectWithTag("Spear");

        inventory = FindAnyObjectByType<Inventory>();

        StartCoroutine(RandomMovement());
    }

    void Update()
    {
        if (isCaught)
            return;

        CheckFear();
    }

    void FixedUpdate()
    {
        if (isCaught)
            return;

        float currentSpeed = isPanicking ? panicSpeed : moveSpeed;

        rb.linearVelocity = moveDirection * currentSpeed;

        CheckWaterBounds();

        // Flip sesuai arah gerak
        if (moveDirection.x > 0.1f)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (moveDirection.x < -0.1f)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    IEnumerator RandomMovement()
    {
        while (!isCaught)
        {
            // jangan random saat panic
            if (!isPanicking)
            {
                moveDirection = Random.insideUnitCircle.normalized;
            }

            float waitTime = Random.Range(
                minChangeTime,
                maxChangeTime
            );

            yield return new WaitForSeconds(waitTime);
        }
    }

    void CheckFear()
    {
        if (Spear.instance == null)
        return;

        player = Spear.instance.transform;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectDistance && !isPanicking)
        {
            StartCoroutine(Panic());
        }
    }


    IEnumerator Panic()
    {
        isPanicking = true;

        // arah kabur dari player
        Vector2 escapeDirection =
            (transform.position - player.position).normalized;

        moveDirection = escapeDirection;

        yield return new WaitForSeconds(panicDuration);

        isPanicking = false;
    }

    void CheckWaterBounds()
    {
        if (waterArea == null)
            return;

        if (!waterArea.bounds.Contains(transform.position))
        {
            Vector2 center = waterArea.bounds.center;

            moveDirection =
                (center - (Vector2)transform.position).normalized;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (isCaught)
        return;
    if (collision.CompareTag("Spear") &&
        Spear.instance.IsReturning == false)
    {
        Debug.Log("Ikan tertangkap!");

        isCaught = true;

        rb.linearVelocity = Vector2.zero;

        inventory.AddFish();
        BestiaryManager.instance.RegisterFish(fishData);

        StartCoroutine(
            AttachToSpear(collision.transform)
        );
    }
}

    IEnumerator AttachToSpear(Transform spear)
    {
        yield return new WaitForSeconds(0.1f);
        transform.SetParent(spear);
        transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(0.3f);
    }

    void OnDrawGizmosSelected()
    {
        // Area air
        if (waterArea != null)
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireCube(
                waterArea.bounds.center,
                waterArea.bounds.size
            );
        }

        // Radius takut
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectDistance
        );
    }
}