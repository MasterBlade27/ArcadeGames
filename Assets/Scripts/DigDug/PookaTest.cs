using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class PookaTest : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private int xDir = 0;
    [SerializeField] 
    private int yDir = 0;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private float moveSpeed = 6f;
    [SerializeField]
    private float idleTime = 2f;
    [SerializeField]
    private Vector3 moveDirection;

    [SerializeField]
    private float cellSize = 10f;

    private const float ArriveEpsilon = 0.05f;

    [SerializeField, Range(0f, 1f)]
    private float perpendicularPreference = 0.5f;

    [SerializeField]
    private float checkDistance = 6f;

    [SerializeField]
    private float chaseDuration = 4f;
    [SerializeField]
    private float chaseStopDistance = 1.5f;

    [SerializeField]
    private LayerMask obstacleLayer;

    // Ghost mode settings
    [SerializeField]
    private float ghostChance = 0.3f;
    [SerializeField]
    private float ghostDuration = 2f;
    [SerializeField]
    private float ghostSearchRadius = 5f;
    [SerializeField]
    private float ghostCooldown = 1.5f; // cooldown after completing a ghost move

    private float idleTimer;
    private float chaseTimer;
    private bool isChasing = false;
    private bool isGhost = false;
    private float ghostTimer;
    private float ghostCooldownTimer;
    private Collider pookaCollider;
    public EnemyPump pump;

    void Start()
    {
        player = FindAnyObjectByType<DigMovement>()?.gameObject ?? GameObject.FindGameObjectWithTag("Player");
        idleTimer = idleTime;
        pookaCollider = GetComponent<Collider>();
        moveDirect();
        targetPosition = new Vector3(transform.position.x + xDir * cellSize, 
             transform.position.y, transform.position.z + yDir * cellSize);
        pump = GetComponent<EnemyPump>();
    }

    void Update()
    {
        while (pump.Inflate > 0)
        {
            // If inflated, do not move
            return;
        }
        // Update ghost mode timer
        if (isGhost)
        {
            ghostTimer -= Time.deltaTime;
            if (ghostTimer <= 0f)
            {
                isGhost = false;
                // Re-enable collider when ghost mode ends
                if (pookaCollider != null)
                    pookaCollider.enabled = true;
                // Start cooldown after ghost move completes
                ghostCooldownTimer = ghostCooldown;
            }
        }

        // Update ghost cooldown
        if (ghostCooldownTimer > 0f)
        {
            ghostCooldownTimer -= Time.deltaTime;
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                             new Vector2(targetPosition.x, targetPosition.z)) < ArriveEpsilon)
        {
            // arrived at a grid cell
            if (isChasing)
            {
                chaseTimer -= Time.deltaTime;
                if (player == null || chaseTimer <= 0f || Vector3.Distance(transform.position, player.transform.position) <= chaseStopDistance)
                {
                    isChasing = false;
                    isGhost = false;
                    if (pookaCollider != null)
                        pookaCollider.enabled = true;
                    idleTimer = idleTime;
                }
            }
            else
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0f)
                {
                    isChasing = true;
                    chaseTimer = chaseDuration;
                }
            }

            // Only call moveDirect if not in ghost mode
            // This prevents re-pathing while the Pooka is moving through a wall
            if (!isGhost)
            {
                moveDirect();
                targetPosition = new Vector3(transform.position.x + xDir * cellSize,
                     transform.position.y, transform.position.z + yDir * cellSize);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void moveDirect()
    {
        // test which cardinal directions are free using raycasts against obstacle layer only
        bool up = !Physics.Raycast(transform.position, Vector3.forward, checkDistance, obstacleLayer);
        bool down = !Physics.Raycast(transform.position, Vector3.back, checkDistance, obstacleLayer);
        bool right = !Physics.Raycast(transform.position, Vector3.right, checkDistance, obstacleLayer);
        bool left = !Physics.Raycast(transform.position, Vector3.left, checkDistance, obstacleLayer);

        // If we're in chase mode, try to pick a direction that reduces distance to the player
        if (isChasing && player != null)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            bool preferHorizontal = Mathf.Abs(toPlayer.x) > Mathf.Abs(toPlayer.z);
            
            if (preferHorizontal)
            {
                int wantX = toPlayer.x > 0 ? 1 : -1;
                if ((wantX == 1 && right) || (wantX == -1 && left))
                {
                    xDir = wantX; yDir = 0;
                    return;
                }
                // if preferred horizontal blocked, allow perpendiculars (vertical)
                var perpChoices = new List<Vector2Int>();
                if (up) perpChoices.Add(new Vector2Int(0, 1));
                if (down) perpChoices.Add(new Vector2Int(0, -1));
                if (perpChoices.Count > 0 && Random.value <= perpendicularPreference)
                {
                    var pick = perpChoices[Random.Range(0, perpChoices.Count)];
                    xDir = pick.x; yDir = pick.y; return;
                }

                // If still blocked and not in ghost, try to enter ghost mode (only if cooldown expired)
                if (!isGhost && ghostCooldownTimer <= 0f && Random.value <= ghostChance)
                {
                    AttemptGhostMode(wantX, 0);
                    if (isGhost) return;
                }
            }
            else
            {
                int wantZ = toPlayer.z > 0 ? 1 : -1;
                if ((wantZ == 1 && up) || (wantZ == -1 && down))
                {
                    yDir = wantZ; xDir = 0;
                    return;
                }
                var perpChoices = new List<Vector2Int>();
                if (right) perpChoices.Add(new Vector2Int(1, 0));
                if (left) perpChoices.Add(new Vector2Int(-1, 0));
                if (perpChoices.Count > 0 && Random.value <= perpendicularPreference)
                {
                    var pick = perpChoices[Random.Range(0, perpChoices.Count)];
                    xDir = pick.x; yDir = pick.y; return;
                }

                // If still blocked and not in ghost, try to enter ghost mode (only if cooldown expired)
                if (!isGhost && ghostCooldownTimer <= 0f && Random.value <= ghostChance)
                {
                    AttemptGhostMode(0, wantZ);
                    if (isGhost) return;
                }
            }
        }

        // helper to collect available perpendiculars relative to current movement
        var perpendiculars = new List<Vector2Int>();
        if (Mathf.Abs(xDir) > 0)
        {
            if (up) perpendiculars.Add(new Vector2Int(0, 1));
            if (down) perpendiculars.Add(new Vector2Int(0, -1));
        }
        else if (Mathf.Abs(yDir) > 0)
        {
            if (right) perpendiculars.Add(new Vector2Int(1, 0));
            if (left) perpendiculars.Add(new Vector2Int(-1, 0));
        }
        else
        {
            if (up) perpendiculars.Add(new Vector2Int(0, 1));
            if (down) perpendiculars.Add(new Vector2Int(0, -1));
            if (right) perpendiculars.Add(new Vector2Int(1, 0));
            if (left) perpendiculars.Add(new Vector2Int(-1, 0));
        }

        bool currentIsClear = (xDir == 1 && right) || (xDir == -1 && left) || (yDir == 1 && up) || (yDir == -1 && down);

        if (currentIsClear)
        {
            if (perpendiculars.Count > 0 && Random.value <= perpendicularPreference)
            {
                var pick = perpendiculars[Random.Range(0, perpendiculars.Count)];
                xDir = pick.x;
                yDir = pick.y;
                return;
            }
            else
            {
                return;
            }
        }

        var options = new List<Vector2Int>();
        if (up) options.Add(new Vector2Int(0, 1));
        if (down) options.Add(new Vector2Int(0, -1));
        if (right) options.Add(new Vector2Int(1, 0));
        if (left) options.Add(new Vector2Int(-1, 0));

        if (options.Count == 0)
        {
            xDir = 0;
            yDir = 0;
            return;
        }

        var perpAvailable = new List<Vector2Int>();
        foreach (var p in options)
            if (perpendiculars.Contains(p))
                perpAvailable.Add(p);

        if (perpAvailable.Count > 0 && Random.value <= perpendicularPreference)
        {
            var pick = perpAvailable[Random.Range(0, perpAvailable.Count)];
            xDir = pick.x;
            yDir = pick.y;
            return;
        }

        var choice = options[Random.Range(0, options.Count)];
        xDir = choice.x;
        yDir = choice.y;
    }

    /// <summary>
    /// When blocked during chase, enter ghost mode and move through walls to the nearest clear tile.
    /// Finds the closest clear cell within ghostSearchRadius and locks to that target until arrival.
    /// No break when arriving at the ghost destination - continues seamlessly.
    /// </summary>
    private void AttemptGhostMode(int dirX, int dirZ)
    {
        Vector3 currentGrid = new Vector3(Mathf.Round(transform.position.x / cellSize) * cellSize,
                                         transform.position.y,
                                         Mathf.Round(transform.position.z / cellSize) * cellSize);

        Vector3 nearestClearCell = Vector3.zero;
        float closestDistance = float.MaxValue;

        // Search in the desired direction only (not a full radius)
        int searchSteps = (int)ghostSearchRadius;
        for (int step = 1; step <= searchSteps; step++)
        {
            Vector3 candidate = currentGrid + new Vector3(dirX * step * cellSize, 0f, dirZ * step * cellSize);

            // Check if this cell is clear (no obstacles blocking cardinal directions from it)
            bool isClear = !Physics.Raycast(candidate, Vector3.forward, checkDistance, obstacleLayer) &&
                           !Physics.Raycast(candidate, Vector3.back, checkDistance, obstacleLayer) &&
                           !Physics.Raycast(candidate, Vector3.right, checkDistance, obstacleLayer) &&
                           !Physics.Raycast(candidate, Vector3.left, checkDistance, obstacleLayer);

            if (isClear)
            {
                // Found a clear cell: store the first one as the target
                // (This is the nearest clear cell in the desired direction)
                nearestClearCell = candidate;
                closestDistance = Vector3.Distance(currentGrid, candidate);
                break; // Take the first clear cell found (nearest in that direction)
            }
        }

        // Only enter ghost mode if we found a clear cell ahead
        if (closestDistance < float.MaxValue)
        {
            // Set target and lock direction
            targetPosition = nearestClearCell;
            xDir = dirX;
            yDir = dirZ;

            // Disable collider so the Pooka passes through walls
            if (pookaCollider != null)
                pookaCollider.enabled = false;

            isGhost = true;
            ghostTimer = ghostDuration;
        }
    }
}
