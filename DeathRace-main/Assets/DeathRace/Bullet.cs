using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 2.0f;

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
        if (other.transform.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.TakeDamage(10);
            ObjectPooling.Instance.SetPoolObject(gameObject, gunBehaviour.bulletObjectType);
        }
    }
}
