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
        // Appelle la méthode pour relancer la génération de la carte
        dungeonGenerator.ReplayDungeon();

        // Réinitialisez le joueur
        ResetPlayer();

        // Réinitialisez le CoinManager
        coinManager.Initialize();

        // Cachez l'UI de gameOver après avoir cliqué sur redémarrer
        gameOverUI.SetActive(false);
    }
    
    public void startgame()
    {
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
}
