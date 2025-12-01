using UnityEngine;
using UnityEngine.Events;

public class PlayerHandler : MonoBehaviour
{
    public UnityEvent<Vector3> OnPlayerPositionChanged;
    private Rigidbody playerRigidbody;
    private Vector3 previousPosition;
    [SerializeField] private float positionThreshold = 0.001f;
    [SerializeField] private float bounceForce = 10f;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        if ((currentPosition - previousPosition).sqrMagnitude > positionThreshold * positionThreshold)
        {
            previousPosition = currentPosition;
            OnPlayerPositionChanged?.Invoke(currentPosition);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                var v = playerRigidbody.linearVelocity;
                v.y = bounceForce;
                playerRigidbody.linearVelocity = v;
                return;
            }
        }
    }

    public void TeleportTo(Vector3 pos)
    {
        transform.position = pos;
        playerRigidbody.linearVelocity = Vector3.zero;
    }
}
