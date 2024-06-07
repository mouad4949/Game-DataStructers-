using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    public int coinsToDestroyDoors = 10; // Nombre de pièces nécessaires pour détruire les portes
    public int enemiesKilled;
    public int enemiesToDestroyDoors = 5; // Nombre d'ennemis à tuer pour détruire les portes
    private List<GameObject> doors; // Liste des portes à détruire
    public List<GameObject> coins;

    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        doors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));
        coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coins")); // Référencer toutes les pièces avec le tag "Coins"
        coinCount = 0;
        enemiesKilled = 0;
        UpdateCoinText();
        Debug.Log("Initial number of coins: " + coins.Count);
    }

    public void Update()
    {
        coinText.text = "Coins: " + coinCount.ToString();
        if (coinCount >= coinsToDestroyDoors && enemiesKilled >= enemiesToDestroyDoors)
        {
            DestroyDoors();
        }
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }
    
    public List<GameObject> FindCoinsInRadius(Vector3 playerPosition, float radius)
    {
        List<GameObject> nearbyCoins = new List<GameObject>();
        foreach (GameObject coin in coins)
        {
            if (coin != null)
            {
                float distance = Vector3.Distance(playerPosition, coin.transform.position);
                if (distance <= radius)
                {
                    nearbyCoins.Add(coin);
                    Debug.Log("Coin at " + coin.transform.position + " is within detection radius.");
                }
            }
        }
        Debug.Log("Found " + nearbyCoins.Count + " coins within detection radius.");
        return nearbyCoins;
    }

    public void CollectCoin(GameObject coin)
    {
        if (coin != null)
        {
            coins.Remove(coin);
            Destroy(coin);
            coinCount++;
            Debug.Log("Coin collected. Total coins: " + coinCount);
        }
    }

    private void DestroyDoors()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                Destroy(door);
            }
        }
        doors.Clear(); // Vider la liste après destruction
    }
}
