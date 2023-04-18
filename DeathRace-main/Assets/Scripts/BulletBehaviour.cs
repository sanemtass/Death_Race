using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UmbrellaUtils.Pool;
using UmbrellaUtils.Pool.Factory;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform carTransform;

    private Pool<BulletPool> bulletPool;

    private void Start()
    {
        bulletPool = new Pool<BulletPool>(new PrefabFactory<BulletPool>(bulletPrefab, transform), 10);
        CreateBullet();
    }

    private async void CreateBullet()
    {
        while (true)
        {
            await UniTask.Delay(3000);
            var bullet = bulletPool.Allocate();
            bullet.gameObject.SetActive(true);
            bullet.transform.DOMove(carTransform.position, 2).OnComplete(() =>
            {
                bulletPool.Release(bullet);
            });
        }
        
    }

    private void DestroyedBullet()
    {

    }
}
