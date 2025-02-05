using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    public void TakeTurn()
    {
        // Basic enemy behavior - can be expanded later
        // For now, just log
        Debug.Log($"Enemy {gameObject.name} takes turn");
    }
}
