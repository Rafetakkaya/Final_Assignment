using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour


{



    public Camera playerCamera;
    //shooting
    public bool isShooting, readytoShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime=3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto

    }
    public ShootingMode currentShootingMode;
    private void Avake()
    {
        readytoShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ////Left mouse click
        //if(Input.GetKeyDown(KeyCode.Mouse0)) {
        //    FireWeapon();
        //}

        if (currentShootingMode == ShootingMode.Auto)
        {
            //holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode==ShootingMode.Single) {
            //click left mouse button once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
          }
        if(readytoShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
        
    }

    private void FireWeapon()

    {
        readytoShoot=false;

        Vector3 shootingDirection=CalculateDirectionAndSpread().normalized;
        //Instance the bullet
        GameObject bullet=Instantiate(bulletPrefab,bulletSpawn.position,Quaternion.identity);
       
        //poiting the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //shot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized* bulletVelocity,ForceMode.Impulse);
        //destroy the bullet some time
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        //checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShoot", shootingDelay);
            allowReset = false;
        }


        //burst mode
        if(currentShootingMode== ShootingMode.Burst && burstBulletsLeft > 1)//we already shoot once before this check
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }
    private void ResetShoot()
    {
        readytoShoot=true;
        allowReset=true;

    }
    public Vector3 CalculateDirectionAndSpread()
    {
        //shooting from the middle of the screen to check where are we pointing
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
        {
            //hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
         }
        Vector3 directiion=targetPoint-bulletSpawn.position;
        float x =UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        //Returning the shooting directiin and spread
        return directiion + new Vector3(x,y,0);

    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
       yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
