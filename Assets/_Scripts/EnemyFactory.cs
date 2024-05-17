using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    
    [SerializeField] private GameObject enemyPrefab;

    public GameObject CreateEnemy(Vector3 position, Transform parent)
    {
        // Créer une nouvelle instance de l'ennemi à la position spécifiée
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity, parent);
        
        // Vous pouvez ajouter d'autres configurations ici, comme la définition de la vie, le type d'ennemi, etc.

        return newEnemy;
    }
}