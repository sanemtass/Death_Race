using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 2.0f;

    public BulletTag bulletTag;

    private Rigidbody rb;
    public GunBehaviour gunBehaviour;

    public Vector3 targetPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterLifetime());
        targetPos = Vector3.zero;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPooling.Instance.SetPoolObject(gameObject, gunBehaviour.bulletObjectType);
    }

    private void Update()
    {
        if (targetPos != null)
        {
            float step = speed * Time.deltaTime;
            Vector3 atisYonu = targetPos - transform.position;
            atisYonu.Normalize();
            rb.velocity = atisYonu * speed;
        }
    }

    public void GetTarget(Vector3 target)
    {
        targetPos = target;
    }

    public void ResetTarget()
    {
        targetPos = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gunBehaviour != null && other.gameObject != gunBehaviour.gameObject && other.transform.TryGetComponent<IDamagable>(out var damagable))
        {
            if (bulletTag == BulletTag.Level1)
            {
                damagable.TakeDamage(2);
            }
            else if(bulletTag == BulletTag.Level2)
            {
                damagable.TakeDamage(1);
            }
            else if (bulletTag == BulletTag.Level3)
            {
                damagable.TakeDamage(3);
            }
            else if (bulletTag == BulletTag.Level4)
            {
                damagable.TakeDamage(4);
            }
            else if (bulletTag == BulletTag.Level5)
            {
                damagable.TakeDamage(5);
            }
            else if (bulletTag == BulletTag.Level6)
            {
                damagable.TakeDamage(10);
            }

            //damagable.TakeDamage(10);
            ObjectPooling.Instance.SetPoolObject(gameObject, gunBehaviour.bulletObjectType);
        }
    }


}


public enum BulletTag
{
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6
}