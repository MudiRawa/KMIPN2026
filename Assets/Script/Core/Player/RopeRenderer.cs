using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Transform startPoint;
    public Transform endPoint;

    public int segmentCount = 20;
    public float curveHeight = 1.5f;
    public float ropeSag = 0f;
    public float maxSag = 2f;
    public float sagSpeed = 3f;

    public Spear currentHarpoon;

    void Update()
    {
        if (endPoint == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        float distance =
            Vector2.Distance(
                startPoint.position,
                endPoint.position
        );

        // target kelengkungan
        float targetSag = 0f;

        if (currentHarpoon != null)
        {
            if (currentHarpoon.IsStopped)
            {
                targetSag = distance * 0.2f;
            }
        }

        // batasi maksimum
        targetSag = Mathf.Clamp(
            targetSag,
            0,
            maxSag
        );

        // transisi smooth
        ropeSag = Mathf.Lerp(
            ropeSag,
            targetSag,
            Time.deltaTime * sagSpeed
        );

        DrawRope();
    }

    void DrawRope()
    {
        lineRenderer.positionCount = segmentCount;

        Vector3 p0 = startPoint.position;

        Vector3 p2 = endPoint.position;

        Vector3 midPoint = (p0 + p2) / 2;

        // Membuat lengkungan ke bawah
        midPoint.y -= ropeSag;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);

            Vector3 point =
                Mathf.Pow(1 - t, 2) * p0 +
                2 * (1 - t) * t * midPoint +
                Mathf.Pow(t, 2) * p2;

            lineRenderer.SetPosition(i, point);
        }
    }
}