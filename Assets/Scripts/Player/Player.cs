using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Grid grid;
    private bool isPlayerTurn = true;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3Int currentGridPosition;
    
    void Start()
    {
        if (grid == null)
        {
            grid = GameManager.Instance.grid;
        }

        gameObject.tag = "Player";
        
        // Snap to grid on start
        currentGridPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(currentGridPosition);
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isPlayerTurn || isMoving) return;

        var inputDirection = Vector3Int.zero;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))    inputDirection.y = 1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))  inputDirection.y = -1;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))  inputDirection.x = -1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) inputDirection.x = 1;

        if (inputDirection != Vector3Int.zero)
        {
            TryMove(inputDirection);
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Smooth movement to target position
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                moveSpeed * Time.fixedDeltaTime
            );

            // Check if we've reached the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
                EndTurn();
            }
        }
    }

    void TryMove(Vector3Int direction)
    {
        Vector3Int newGridPosition = currentGridPosition + direction;
        Vector3 newWorldPosition = grid.GetCellCenterWorld(newGridPosition);
        Collider2D hitCollider = Physics2D.OverlapCircle(newWorldPosition, 0.1f);

        if (hitCollider == null || (!hitCollider.CompareTag("Enemy") && !hitCollider.CompareTag("Wall")))
        {
            currentGridPosition = newGridPosition;
            targetPosition = newWorldPosition;
            isMoving = true;
        }
    }

    void EndTurn()
    {
        isPlayerTurn = false;
        GameManager.Instance.NextTurn();
    }

    public void StartTurn()
    {
        isPlayerTurn = true;
    }
}
