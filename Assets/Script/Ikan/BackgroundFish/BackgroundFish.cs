using UnityEngine;

public class BackgroundFish : MonoBehaviour
{
    public float minSpeed = 1.5f;
    public float maxSpeed = 3f;

    float speed;

    Vector2 direction;

    public void Initialize(Vector2 moveDirection)
    {
        speed = Random.Range(minSpeed, maxSpeed);
        direction = moveDirection.normalized;

        if (direction.x > 0)
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        else
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}