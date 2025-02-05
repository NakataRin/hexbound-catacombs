using UnityEngine;

// This is an empty class that servers as a proof of concept
public class Enemy : BaseEnemy
{
    public bool IsMoving => isMoving;

    public override void TakeTurn()
    {
        base.TakeTurn();

        Debug.Log($"Overriden turn for {gameObject.name}");
    }
}
