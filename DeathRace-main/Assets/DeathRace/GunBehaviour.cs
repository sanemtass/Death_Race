using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask layermask;

    public Gun gun;
    public GameObject bulletObject;
    public Transform spawnPosition;
    public RaycastHit hit;

    private bool isFiring;
    private bool isEnemyPresent;
    private EnemyCarBehaviour[] enemyCars;
    public Gun nextGun;


    private void Start()
    {
        enemyCars = FindObjectsOfType<EnemyCarBehaviour>();
        InvokeRepeating("CheckForEnemies", 0f, 0.5f);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        CreateBullet();
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    private async UniTask CreateBullet()
    {
        while (isFiring)
        {
            if (isEnemyPresent)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(gun.fireRate));

                if (spawnPosition == null)
                {
                    break;
                }

                var bullet = ObjectPooling.Instance.GetBullet();

                if (bullet == null)
                {
                    break;
                }

                bullet.transform.position = spawnPosition.position;
                bullet.gameObject.SetActive(true);
                bullet.GetComponent<Bullet>().GetTarget(hit.point);
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1));
            }
        }
    }

    private void CheckForEnemies()
    {
        bool foundEnemy = false;
        foreach (var enemy in enemyCars)
        {
            if (enemy != null)
            {
                foundEnemy = true;
                break;
            }
        }

        if (foundEnemy && !isFiring)
        {
            isEnemyPresent = true;
            StartFiring();
        }
        else if (!foundEnemy && isFiring)
        {
            isEnemyPresent = false;
            StopFiring();
        }
    }
}
