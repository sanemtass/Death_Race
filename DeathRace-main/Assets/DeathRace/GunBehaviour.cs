using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask layermask;

    public Gun gun;
    public int bulletObjectType;
    public Transform spawnPosition;
    public RaycastHit hit;

    private bool isFiring;
    private bool isEnemyPresent;
    private EnemyCarBehaviour[] enemyCars;
    public Gun nextGun;
    public Vector3 gunRotation=new Vector3();

    private void Start()
    {
        enemyCars = FindObjectsOfType<EnemyCarBehaviour>();
        transform.localRotation = Quaternion.Euler(gunRotation);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layermask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (!isFiring)
            {
                //transform.LookAt(hit.transform);
                isEnemyPresent = true;
                StartFiring();
            }
        }
        else
        {
            if (isFiring)
            {
                isEnemyPresent = false;
                StopFiring();
            }
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

                var bullet = ObjectPooling.Instance.GetPoolObject(bulletObjectType);

                if (bullet == null)
                {
                    break;
                }

                bullet.transform.position = spawnPosition.position;
                bullet.gameObject.SetActive(true);

                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.gunBehaviour = this;
                bulletComponent.GetTarget(hit.point);
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.1));
            }
        }
    }
}
