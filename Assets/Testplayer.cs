using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testplayer : MonoBehaviour
{

    //Gun variable
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    //optional control fire rate
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;
    private float fireTimer;


    private void Update()
    {
        HandleRotation();
        Shoot();

    }

    private void HandleRotation()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        // rb.rotation = angle; same used more with physics "gravity"
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }


    private void Shoot(){
        if(Input.GetMouseButton(0) && fireTimer<=0f){
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            fireTimer = fireRate;
        } else {
            fireTimer -= Time.deltaTime;
        }
    }

}
