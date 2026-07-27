using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletLiveTime;
    public GameObject hitEffectPrefab;

    void OnEnable()
    {
        Invoke("Deactivate", bulletLiveTime);
    }

   private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BaseEnemy>(out BaseEnemy enemy)) 
        {
            if (hitEffectPrefab != null) 
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }
            enemy.TakeDamage(12);
            gameObject.SetActive(false);
            CancelInvoke("Deactivate");
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}
