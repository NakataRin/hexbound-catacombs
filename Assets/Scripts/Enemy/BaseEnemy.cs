using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    protected bool isMoving = false;
    protected Vector3Int currentGridPosition;
    protected Vector3 targetPosition;
    protected Grid grid;

    protected virtual void Start()
    {
        grid = FindAnyObjectByType<Grid>();
        if (grid == null)
        {
            Debug.LogError($"No Grid found for enemy: {gameObject.name}");
            enabled = false;
            return;
        }

        // Snap to grid on start
        currentGridPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(currentGridPosition);
        targetPosition = transform.position;
    }

    public virtual void TakeTurn()

    {
        // Base turn behavior. TODO: Implement enemy AI/Leave empty to redifine for each enemy
        Debug.Log($"Base turn for {gameObject.name}");
    }

    protected virtual void Move(Vector3Int direction)
    {
        if (grid == null)
        {
            Debug.LogError($"No Grid found for enemy: {gameObject.name}");
            enabled = false;
            return;
        }

        Vector3 newPosition = grid.GetCellCenterWorld(
            grid.WorldToCell(transform.position) + direction
        );

        // Check for collisions
        if (!Physics2D.OverlapCircle(newPosition, 0.1f))
        {
            targetPosition = newPosition;
            isMoving = true;
        }
    }

    protected virtual void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}
