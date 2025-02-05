using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Player player;
    private List<Enemy> enemies = new List<Enemy>();
    private bool isProcessingTurn = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find all enemies in the scene
        enemies.AddRange(FindObjectsByType<Enemy>(FindObjectsSortMode.None));
        if (enemies.Count == 0)
        {
            Debug.LogWarning("No enemies found in the scene");
        }

        // Ensure player reference is set
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
            if (player == null)
            {
                Debug.LogError("No player found in the scene");
                enabled = false;
                return;
            }
        }
    }


    public void NextTurn()
    {
        if (isProcessingTurn) return;
        
        StartCoroutine(ProcessTurn());
    }

    private IEnumerator ProcessTurn()
    {
        isProcessingTurn = true;

        // Process enemy turns one by one
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeTurn();
            yield return new WaitForSeconds(0.5f); // Add delay between enemy moves
        }

        // Wait a bit after all enemies move
        yield return new WaitForSeconds(0.2f);

        // Return to player's turn
        isProcessingTurn = false;
        player.StartTurn();
    }

    // Helper method to check if any enemies are still moving
    public bool AreEnemiesMoving()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsMoving)
                return true;
        }
        return false;
    }
}
