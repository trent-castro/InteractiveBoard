using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Turret : MonoBehaviour
{
    [SerializeField] Transform spawnLocation = null;
    [SerializeField] ObjectPool bulletPool = null;
    [SerializeField] bool DebugMode = false;



    private void Update()
    {
        if (DebugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Firing Bullet");
                FireBullet();
            }
        }
    }

    public void FireBullet()
    {
        GameObject bullet = bulletPool.GetNextPooledObject();
        bullet.transform.position = spawnLocation.position;
        bullet.gameObject.SetActive(true);

        Vector3 fireDirection = spawnLocation.position - gameObject.transform.position;

        bullet.GetComponent<ABR_Bullet>().Fire(fireDirection);

    }

}
