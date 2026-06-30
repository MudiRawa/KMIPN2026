using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject harpoonPrefab;
    public Transform gunPoint;
    public LineRenderer lineRenderer;
    public RopeRenderer ropeRenderer;
    public Camera mainCamera;
    public float aimOffsetAngle = 0f;

    public bool isShooting;

    private GameObject currentHarpoon;

    public static PlayerShoot instance;

    void Start()
    {
        instance = this;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (PlayerMovement.instance.isUnderwater == false)
        {
            return;
        }
        Aim();
    }

    void Aim()
    {
        if (gunPoint == null || mainCamera == null)
            return;

        // Project the mouse position onto a plane that passes through the gun point
        // and is oriented parallel to the camera view. This ensures we only aim
        // in X and Y (screen space) while working in a 3D world.
        Plane plane = new Plane(mainCamera.transform.forward, gunPoint.position);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float enter;
        Vector3 mouseWorldPosition;
        if (plane.Raycast(ray, out enter))
        {
            mouseWorldPosition = ray.GetPoint(enter);
        }
        else
        {
            return;
        }

        Vector3 direction = mouseWorldPosition - gunPoint.position;
        direction.z = 0f; // explicitly ignore depth for 2D aiming
        if (direction.sqrMagnitude <= 0.0001f)
            return;

        bool facingRight = PlayerMovement.instance == null || PlayerMovement.instance.FacingRight;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (facingRight)
        {
            if (angle > 180f)
                angle -= 360f;

            angle = Mathf.Clamp(angle, -50f + aimOffsetAngle, 50f + aimOffsetAngle);
        }
        else
        {
            angle = Mathf.Repeat(angle + 360f, 360f);
            angle = Mathf.Clamp(angle, 130f + aimOffsetAngle, 230f + aimOffsetAngle);
        }

        gunPoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Shoot()
    {
        if (currentHarpoon != null || isShooting)
            return;
        
        isShooting = true;

        currentHarpoon = Instantiate(harpoonPrefab, gunPoint.position, gunPoint.rotation);
        ropeRenderer.endPoint = currentHarpoon.transform;

        Spear harpoonScript = currentHarpoon.GetComponent<Spear>();
        ropeRenderer.currentHarpoon = harpoonScript;

        harpoonScript.Setup(transform);
        harpoonScript.OnHarpoonReturned += ResetHarpoon;
    }

    void ResetHarpoon()
    {
        currentHarpoon = null;
        ropeRenderer.endPoint = null;
        isShooting = false;
    }
}