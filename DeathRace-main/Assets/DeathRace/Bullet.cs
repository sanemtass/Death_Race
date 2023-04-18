using System;
using System.Collections;
using UmbrellaUtils.Pool;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 2.0f; // Mermi ömrünü belirleyin

    private Rigidbody rb;
    private GunBehaviour gunBehaviour;

    public Vector3 targetPos;

    private UIManager uIManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        uIManager = GetComponent<UIManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterLifetime());
        targetPos = Vector3.zero;
    }

    private IEnumerator DestroyAfterLifetime()
    {
        // Belirlenen süre kadar bekleyin
        yield return new WaitForSeconds(lifetime);

        // Süre dolduğunda mermiyi pool'a geri gönderin
        ObjectPooling.Instance.ReturnBullet(gameObject);
    }

    private void Update()
    {
        if (targetPos != null)
        {
            float step = speed * Time.deltaTime;
            Vector3 atisYonu = targetPos - transform.position; // Atış yönü, çarpma noktası ile merminin pozisyonu arasındaki fark
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
            ObjectPooling.Instance.ReturnBullet(gameObject);
        }
    }
}
