using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinFactory : MonoBehaviour
{
    
    [SerializeField] private GameObject coin;

    public GameObject CreateCoin(Vector3 position, Transform parent)
    {
        // Créer une nouvelle instance de l'ennemi à la position spécifiée
        GameObject newEnemy = Instantiate(coin, position, Quaternion.identity, parent);
        
        // Vous pouvez ajouter d'autres configurations ici, comme la définition de la vie, le type d'ennemi, etc.

        return newEnemy;
    }
}