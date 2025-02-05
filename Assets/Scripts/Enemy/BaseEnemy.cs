using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    protected bool isMoving = false;
    public bool IsMoving => isMoving;
    protected Vector3Int currentGridPosition;
    protected Vector3 targetPosition;
    protected Grid grid;
    const float COLLISION_CHECK_RADIUS = 0.1f;

    /// <summary>
    /// Initialize the enemy. Snap to grid on start. Add a circle collider if one doesn't exist.
    /// </summary>
    protected virtual void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found in scene");
            enabled = false;
            return;
        }

        grid = GameManager.Instance.Grid;

        if (grid == null)
        {
            Debug.LogError("Grid not found in scene");
            enabled = false;
            return;
        }

        gameObject.tag = GameManager.ENEMY_TAG;

        SetUpCollider();

        // Ensure enemy is perfectly centered in its starting tile
        currentGridPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(currentGridPosition);
        targetPosition = transform.position;
    }

    private void SetUpCollider()
    {
        if (!TryGetComponent<CircleCollider2D>(out var collider))
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = COLLISION_CHECK_RADIUS;
            collider.isTrigger = true;
        }
    }

    /// <summary>
    /// Base implementation of enemy turn behavior
    /// Override this method in derived classes to implement specific enemy AI
    /// </summary>
    public virtual void TakeTurn()
    {
        // Default implementation logs the turn for debugging
        Debug.Log($"Base turn for {gameObject.name}");
    }


    /// <summary>
    /// Attempts to move the enemy in the given direction, respecting grid alignment and collisions
    /// </summary>
    protected virtual void Move(Vector3Int direction)
    {
        Vector3Int newGridPosition = currentGridPosition + direction;
        Vector3 newWorldPosition = grid.GetCellCenterWorld(newGridPosition);

        Collider2D hitCollider = Physics2D.OverlapCircle(newWorldPosition, COLLISION_CHECK_RADIUS);

        bool isBlocked = hitCollider != null && (
            hitCollider.CompareTag(GameManager.ENEMY_TAG) || 
            hitCollider.CompareTag(GameManager.WALL_TAG) ||
            hitCollider.CompareTag(GameManager.PLAYER_TAG)
        );
        if (!isBlocked)
        {
            currentGridPosition = newGridPosition;
            targetPosition = newWorldPosition;
            isMoving = true;
        }
    }


    /// <summary>
    /// Handles smooth movement between grid positions
    /// </summary>
    protected virtual void Update()
    {
        if (isMoving)
        {
            // Smoothly interpolate to target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Check if we've reached the target position (with small threshold to prevent floating point issues)
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}
