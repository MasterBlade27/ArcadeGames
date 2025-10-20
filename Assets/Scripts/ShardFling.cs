using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShardFling : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> shards;

    // Now accepts the impact velocity so shards can be biased in that direction
    public void Shatter(Vector3 impactVelocity)
    {
        // Normalize the incoming velocity (if zero fallback to up)
        Vector3 impactDir = impactVelocity.sqrMagnitude > 0.0001f ? impactVelocity.normalized : Vector3.up;

        foreach (GameObject shard in shards)
        {
            Rigidbody rb = shard.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // random component (keeps some variety)
            Vector3 randomComponent = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.2f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            // bias the final direction towards the ball's impact direction
            float biasWeight = 0.50f; // how strongly shards follow the ball's direction
            Vector3 finalDir = (impactDir * biasWeight + randomComponent * (1f - biasWeight)).normalized;

            // scale force with some randomness and weakly with impact speed
            float baseForce = Random.Range(5f, 7f);
            float speedFactor = Mathf.Clamp(impactVelocity.magnitude * 0.15f, 0f, 3f);
            float finalForce = baseForce * (1f + speedFactor);

            rb.AddForce(finalDir * finalForce, ForceMode.Impulse);

            float randomTorque = Random.Range(-10f, 10f);
            rb.AddTorque(Random.onUnitSphere * randomTorque, ForceMode.Impulse);
        }
    }
}
