using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed used when moving the player towards the finger/mouse position (units/sec)")]
    [SerializeField] private float moveSpeed = 20f;

    [Tooltip("Padding from screen edges in world units to keep the player visible")]
    [SerializeField] private float edgePadding = 0.5f;

    [Tooltip("Smoothing time for horizontal movement. Set to 0 for immediate following.")]
    [SerializeField] private float smoothTime = 0.06f;

    private Camera mainCam;
    private bool dragging;
    private float screenZ;
    private Vector3 dragOffset;
    private float velocityX;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
            Debug.LogWarning("PlayerMovement: No Camera.main found — clamping will not work correctly.");

        // We'll use the Z of the player in screen space when converting screen->world
        screenZ = mainCam != null ? mainCam.WorldToScreenPoint(transform.position).z : 0f;
    }

    void Update()
    {
        // Handle input for both touch (mobile) and mouse (editor)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
                BeginDrag(t.position);
            else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                ContinueDrag(t.position);
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                EndDrag();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                BeginDrag(Input.mousePosition);
            else if (Input.GetMouseButton(0))
                ContinueDrag(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                EndDrag();
        }
    }

    private void BeginDrag(Vector2 screenPos)
    {
        if (mainCam == null)
            mainCam = Camera.main;

        screenZ = mainCam != null ? mainCam.WorldToScreenPoint(transform.position).z : 0f;

        Vector3 worldPoint = ScreenToWorld(screenPos);
        dragOffset = transform.position - worldPoint;
        dragging = true;
    }

    private void ContinueDrag(Vector2 screenPos)
    {
        if (!dragging)
            return;

        Vector3 worldPoint = ScreenToWorld(screenPos);
        Vector3 target = worldPoint + dragOffset;

        // Keep original y and z
        target.y = transform.position.y;
        target.z = transform.position.z;

        // Clamp horizontally to camera view
        target.x = ClampXToCamera(target.x);

        // Smooth horizontal movement to reduce jerk. We only smooth the X axis.
        float currentX = transform.position.x;
        float targetX = target.x;

        float newX;
        if (smoothTime > 0f)
        {
            newX = Mathf.SmoothDamp(currentX, targetX, ref velocityX, smoothTime);
        }
        else
        {
            // Immediate follow (no smoothing) but still respect moveSpeed to avoid excessive teleporting
            newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void EndDrag()
    {
        dragging = false;
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        if (mainCam == null)
            return Vector3.zero;

        return mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenZ));
    }

    private float ClampXToCamera(float x)
    {
        if (mainCam == null)
            return x;

        // Get left and right world x positions at the player's Z depth
        Vector3 leftWorld = mainCam.ViewportToWorldPoint(new Vector3(0f, 0.5f, screenZ));
        Vector3 rightWorld = mainCam.ViewportToWorldPoint(new Vector3(1f, 0.5f, screenZ));

        float minX = leftWorld.x + edgePadding;
        float maxX = rightWorld.x - edgePadding;

        return Mathf.Clamp(x, minX, maxX);
    }
}
