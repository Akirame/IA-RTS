using UnityEngine;
using System.Collections;

public class BoidBehaviour : MonoBehaviour
{
    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.5f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 10.0f)]
    public float neighborDist = 2.0f;

    public Vector3 pivot;

    public Vector2 directionToPoint;

    public LayerMask searchLayer;

    // Random seed.
    float noiseOffset;

    // Caluculates the separation vector with a target.
    Vector3 GetSeparationVector(Transform target)
    {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / neighborDist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        noiseOffset = Random.value * 10.0f;
    }

    void Update()
    {
        GetMouseInput();

        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        // Current velocity randomized with noise.
        var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        var speed = (1.0f + noise * velocityVariation);

        // Initializes the vectors.
        var separation = Vector3.zero;
        var alignment = transform.right;
        var cohesion = transform.position;

        // Looks up nearby boids.
        var nearbyBoids = Physics2D.OverlapCircleAll(currentPosition, neighborDist, searchLayer);
        
        // Accumulates the vectors.
        foreach (var boid in nearbyBoids)
        {
            if (boid.gameObject == gameObject) continue;
            var t = boid.transform;
            separation += GetSeparationVector(t);
            alignment += t.right;
            cohesion += t.position;
        }

        if(nearbyBoids.Length > 0)
        { 
            var avg = 1.0f / nearbyBoids.Length;
            alignment *= avg;
            cohesion *= avg;
            cohesion = (cohesion - currentPosition).normalized;
        }
        // Calculates a rotation from the vectors.
        directionToPoint = (pivot - transform.position).normalized;
        var direction = (Vector3)directionToPoint + separation + alignment + cohesion;
        pivot = direction;
        print(alignment+","+ cohesion+"," + separation+"," + direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        // Applys the rotation with interpolation.
        if (rotation != currentRotation)
        {
            var ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
        }

        // Moves forward.
        transform.position = currentPosition + transform.right * (speed * Time.deltaTime);
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pivot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, neighborDist);
    }
}
