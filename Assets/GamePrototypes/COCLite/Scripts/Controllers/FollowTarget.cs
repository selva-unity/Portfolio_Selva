using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowTarget : MonoBehaviour
{
    [Tooltip("Who should this agent follow? (e.g., player)")]
    public Transform[] targets;

    [Tooltip("How often to update the path (seconds). 0 = every frame.")]
    public float updateInterval = 0.1f;

    private NavMeshAgent _agent;
    private float _timer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    //Updated update method to support nearest target selection, because a single target is moving the agent to rotate around the target.
    private void Update()
    {
        if (targets == null || targets.Length == 0) return;

        // find nearest non-null target
        Transform nearest = null;
        float minSqr = float.MaxValue;
        Vector3 myPos = transform.position;
        foreach (var t in targets)
        {
            if (t == null) continue;
            float sqr = (t.position - myPos).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = t;
            }
        }

        if (nearest == null) return;

        if (updateInterval <= 0f)
        {
            // Update every frame
            _agent.SetDestination(nearest.position);
        }
        else
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _agent.SetDestination(nearest.position);
                _timer = updateInterval;
            }
        }
    }
}