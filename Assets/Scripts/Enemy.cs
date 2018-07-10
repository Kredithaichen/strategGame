using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float watchRadius = 10.0f;
    [SerializeField]
    private float shootRadius = 5.0f;

    private float currentCoolDown;
    [SerializeField]
    private float coolDown = 2.0f;

    [SerializeField]
    private float hitAccuracy = 0.8f;
    [SerializeField]
    private float shotDamage = 5.0f;

    private System.Random randomGenerator;
    private Vector3 lastShotOrigin, lastShotDirection;

    private ParticleSystem shootParticleSystem;

    // Use this for initialization
    void Start()
    {
        randomGenerator = new System.Random();

        shootParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, shootRadius);

        if (currentCoolDown <= 0.0f)
        {
            foreach (var c in colliders)
            {
                if (c.gameObject.GetComponent<EnemyTarget>() != null)
                {
                    var dir = c.transform.position - transform.position;

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, dir, out hit))
                    {
                        Debug.DrawRay(transform.position, dir.normalized * hit.distance, Color.green);

                        if (hit.transform.GetComponent<EnemyTarget>() != null)
                        {
                            if (randomGenerator.NextDouble() < hitAccuracy)
                            {
                                Debug.DrawRay(transform.position, dir, Color.red);

                                var agent = hit.transform.GetComponent<SquadAgent>();
                                if (agent != null)
                                    agent.TakeDamage(shotDamage);
                            }

                            lastShotOrigin = transform.position;
                            lastShotDirection = dir;

                            currentCoolDown = coolDown;

                            var options = new ParticleSystem.EmitParams();

                            var angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

                            if (Mathf.Abs(angle) > 10.0f)
                                transform.RotateAround(transform.position, Vector3.up, angle);
                            shootParticleSystem.Emit(options, 1);

                            break;
                        }
                    }
                }
            }
        }
        else
            currentCoolDown -= Time.deltaTime;

        if (lastShotDirection.magnitude > 0.0f)
            Debug.DrawRay(lastShotOrigin, lastShotDirection, Color.yellow);
    }

    private void Shoot(EnemyTarget target)
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, watchRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRadius);
    }
}
