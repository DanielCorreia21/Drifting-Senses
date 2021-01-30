using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;

    public Transform playerCenter;
    public Transform firingPoint;
    public GameObject bulletPrefab;

    public float fireRate = 2 * 60;
    private float _lastTimeShot;

    private void Start()
    {
        _lastTimeShot = Time.realtimeSinceStartup;
    }


    // Update is called once per frame
    void Update()
    {
        rotateGun();
        if (Input.GetButtonDown("Fire1"))
        {
            if((Time.realtimeSinceStartup - _lastTimeShot) > 60 / fireRate)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerBullet, 1f);
        _lastTimeShot = Time.realtimeSinceStartup;
    }

    private void rotateGun()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = -1.88457f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(playerCenter.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        playerCenter.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
