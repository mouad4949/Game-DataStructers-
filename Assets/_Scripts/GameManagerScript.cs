using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{   
    public GameObject gameOverUI;
    public RoomFirstDungeonGenerator dungeonGenerator;
    public Player player;
    public CoinManager coinManager;
    public GameObject startgameUI;
    public PathfindingManager pathfindingManager; 
    public GridA  Grida;
    public GameObject boss;
    public GameObject WongameUI;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        
        
    }


    public void restart()
    {  
        pathfindingManager.GridA.SetActive(false);   
        Time.timeScale = 1.0f;
        
        pathfindingManager.ResetSkip();
        // Appelle la méthode pour relancer la génération de la carte
        dungeonGenerator.ReplayDungeon();

        // Réinitialisez le joueur
        ResetPlayer();

        // Réinitialisez le CoinManager
        coinManager.Initialize();

        // Cachez l'UI de gameOver après avoir cliqué sur redémarrer
        gameOverUI.SetActive(false);
        WongameUI.SetActive(false);
        boss.SetActive(true);
    }
    
    public void startgame()
    {   Time.timeScale = 1.0f;
        pathfindingManager.DesaactivateGridA();
        // Appelle la méthode pour relancer la génération de la carte
        dungeonGenerator.ReplayDungeon();

        // Réinitialisez le joueur
        ResetPlayer();

        // Réinitialisez le CoinManager
        coinManager.Initialize();

        // Cachez l'UI de gameOver après avoir cliqué sur redémarrer
        startgameUI.SetActive(false);
        
    }

    private void ResetPlayer()
    {
        player.health = player.maxHealth;
        player.isDead = false;
    }
    
    // public void CheckAndActivatePathfinding()
    // {
    //     if (coinManager.coinCount >= coinManager.coinsToDestroyDoors &&
    //         coinManager.enemiesKilled >= coinManager.enemiesToDestroyDoors)
    //     {
    //         pathfindingManager.ActivatePathfinding();
    //         if(pathfindingManager.IsGridAActive()){
    //             pathfindingManager.DeactivatePathfinding();
    //         }
            
    //     }
    // }
    public void CheckAndActivatePathfinding()
    {
        if (coinManager.coinCount >= coinManager.coinsToDestroyDoors &&
            coinManager.enemiesKilled >= coinManager.enemiesToDestroyDoors)
        {
            if (pathfindingManager.IsSkipClicked())
            {
                pathfindingManager.DeactivatePathfinding(); 
                Time.timeScale = 1.0f; // Ensure canvas is hidden if Skip was clicked
            }
            else
            {   Time.timeScale = 0.0f;
                pathfindingManager.ActivatePathfinding();
                if (pathfindingManager.IsGridAActive())
                {   
                    pathfindingManager.DeactivatePathfinding();
                    Time.timeScale = 1.0f;
                }
            }
        }
    }
    
    // public void ActivateGridA()
    // {
    //     Debug.Log("GameManager: Activating GridA");
    //     pathfindingManager.ActivateGridA();
    // }
    public void SkipPathfinding()
    {
        pathfindingManager.Skip();
    }
    
    public void Won()
    {
        Time.timeScale = 0.0f;
        WongameUI.SetActive(true);
    }
}
