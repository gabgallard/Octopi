using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float maxTentacleDistance = 5.0f;
    [SerializeField] LayerMask tentacleTargetLayers;

    [SerializeField] List<TentacleController> tentacles = new List<TentacleController>();

    [SerializeField] ParticleSystem particlesBloodMissile;
    [SerializeField] ParticleSystem particlesBloodBullet;

    float missileForce = 10.0f;
    float bulletForce = 2.0f;

    Rigidbody2D theRigidBody;

    void Awake()
    {
        theRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShootTentacle();
        }
    }

    void OnDrawGizmos()
    {
        var result = RaycastTentacle();

        Gizmos.color = Color.grey;
        Gizmos.DrawLine(result.rayCastIni, result.rayCastEnd);

        // Debug.Log($"direction: {result.direction}");

        if(result.hit)
        {
            Gizmos.DrawSphere(result.hit.transform.position, 0.1f);
        }
    }

    void ShootTentacle()
    {
        RaycastHit2D hit = RaycastTentacle().hit;

        if(hit)
        {
            HookToGrabbable(hit.collider.gameObject.GetComponent<GrabbableController>());
        }
    }

    (Vector2 rayCastIni, Vector2 rayCastEnd, RaycastHit2D hit) RaycastTentacle()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        float distanceToMousePosition = Vector2.Distance(transform.position, mousePosition);

        Vector2 rayCastIni;
        Vector2 rayCastEnd;
        float rayCastDistance;

        if(distanceToMousePosition <= maxTentacleDistance)
        {
            rayCastIni = mousePosition;
            rayCastEnd = mousePosition + (direction * (maxTentacleDistance - distanceToMousePosition));
            rayCastDistance = maxTentacleDistance - distanceToMousePosition;
        } else
        {
            rayCastIni = (Vector2)transform.position + (direction * maxTentacleDistance);
            rayCastEnd = transform.position;
            rayCastDistance = maxTentacleDistance;
            direction = -direction;
        }

        RaycastHit2D hit = Physics2D.Raycast(rayCastIni, direction, rayCastDistance, tentacleTargetLayers);

        return (rayCastIni: rayCastIni, rayCastEnd: rayCastEnd, hit: hit);
    }

    void HookToGrabbable(GrabbableController grabbable)
    {
        TentacleController tentacle = tentacles[Random.Range(0, tentacles.Count)];
        tentacle.Hook(grabbable);
    }

    public void HitByMissile(Vector2 position, Vector2 direction)
    {
        ParticleSystem particles = Instantiate(particlesBloodMissile, position, Quaternion.identity, transform);
        particles.Play();
        Destroy(particles, 10.0f);
        theRigidBody.AddForce(direction * missileForce, ForceMode2D.Impulse);
    }

    public void HitByBullet(Vector2 position, Vector2 direction)
    {
        ParticleSystem particles = Instantiate(particlesBloodBullet, position, Quaternion.identity, transform);
        particles.Play();
        Destroy(particles, 10.0f);
        theRigidBody.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }
}
