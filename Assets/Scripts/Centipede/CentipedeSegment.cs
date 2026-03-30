using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    public Centipede centipede;
    public CentipedeSegment ahead;
    public CentipedeSegment behind;
    public bool isHead => ahead == null;

    public GameObject head;
    public GameObject body;
    private bool headMeshb = false;

    private Vector3 direction = Vector3.right + Vector3.back;
    private Vector3 targetPosition;

    // New: when true the head will dive straight down ignoring obstacles
    private bool isDivingPoison = false;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (isHead && !headMeshb)
        {
            headMeshb = true;
            body.SetActive(false);
            head.SetActive(true);
        }
        if (isHead && Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            UpdateHeadSegement();
        }

        // If mushrooms are healing, freeze centipede movement
        
        float movespeed = (MushroomScoreMarch.regenerating ? 0f : centipede.speed) * Time.deltaTime;

        Vector3 currentPosition = transform.position;
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, movespeed);

        Vector3 movemnentDirection = (targetPosition - currentPosition).normalized;

        // Compute yaw (rotation around Y axis) from the horizontal movement direction
        if (movemnentDirection.sqrMagnitude > 0.0001f)
        {
            // Atan2(x, z) yields angle relative to world forward (z); convert to degrees
            float yaw = Mathf.Atan2(movemnentDirection.x, movemnentDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, -yaw, 0f);
        }

    }

    public void UpdateHeadSegement()
    {
        Vector3 gridPosition = GridPosition(transform.position);

        // If currently diving due to poison, step straight down and ignore obstacle checks
        if (isDivingPoison)
        {
            // ensure we're moving straight down
            direction.x = 0f;
            direction.z = -1f;

            targetPosition.x = gridPosition.x;
            targetPosition.z = gridPosition.z + direction.z;

            Bounds bounds = centipede.homeArea.bounds;
            if (targetPosition.z < bounds.min.z)
                targetPosition.z = bounds.min.z;

            // stop diving when we've reached the bottom row
            if (gridPosition.z <= -36f)
            {
                isDivingPoison = false;
                direction.x = 1f;
            }

            if (behind != null)
                behind.UpdateBodySegment();
            Debug.Log("I'm diving bitch");
            return;
        }

        // Normal horizontal step
        targetPosition.x += direction.x;

        // use a small half-extents box (not zero) and center it on the grid cell
        Vector3 checkCenter = GridPosition(targetPosition);
        Vector3 halfExtents = new Vector3(0.45f, 0.5f, 0.45f); // half extents in world units
        Collider[] hits = Physics.OverlapBox(targetPosition, new Vector3(0, 0, 0), Quaternion.identity, centipede.collisionMask);

        if (hits != null && hits.Length > 0)
        {
            // If any hit is a poisonous mushroom, enter poison-dive mode
            bool foundPoison = false;
            foreach (var col in hits)
            {
                if (col == null) continue;
                Mushroom m = col.GetComponent<Mushroom>();
                if (m != null && m.Poison)
                {
                    foundPoison = true;
                    break;
                }
            }

            if (foundPoison)
            {
                isDivingPoison = true;
                // set to dive down immediately
                direction.x = 0f;
                direction.z = -1f;
                targetPosition.x = gridPosition.x;
                targetPosition.z = gridPosition.z + direction.z;

                Bounds bounds = centipede.homeArea.bounds;
                if (targetPosition.z < bounds.min.z)
                    targetPosition.z = bounds.min.z;
            }
            else
            {
                // regular obstacle response: reverse horizontal dir and step down
                direction.x = -direction.x;

                targetPosition.x = gridPosition.x;
                targetPosition.z = gridPosition.z + direction.z;

                Bounds bounds = centipede.homeArea.bounds;

                if ((direction.z == 1f && targetPosition.z > bounds.max.z) ||
                    (direction.z == -1f && targetPosition.z < bounds.min.z))
                {
                    direction.z = -direction.z;
                    targetPosition.z = gridPosition.z + direction.z;
                }
            }
        }

        if (behind != null)
            behind.UpdateBodySegment();

    }

    public void UpdateBodySegment()
    {
        targetPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;

        if (behind != null)
            behind.UpdateBodySegment();
    }

    private Vector3 GridPosition(Vector3 position)
    {
        position = new Vector3(Mathf.Round(position.x), 0.5f, Mathf.Round(position.z));
        return position;
    }

    private void OnTriggerEnter(Collider collision)
    {

        Debug.Log("Collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Projectile"))
        {
            centipede.Remove(this);
        }
    }
    
}
