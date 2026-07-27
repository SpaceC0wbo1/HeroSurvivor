using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class HeroController : MonoBehaviour
{
    public string heroName;
    public float speed;
    public int damage;
    public int maxHeroHealth;
    public float rotationSpeed = 5f;
    public static event Action<int, int> OnHealthChanged;
    public static event Action OnHeroDied;

    public AudioSource audioSource;
    public AudioClip shootSound;
    public GameObject bulletPrefab;
    public BulletPoolManager bulletPool;

    private Rigidbody rb;
    private Vector3 movementInput;

    private int currentHeroHealth;
    private bool isStuck = false;

    void Start()
    {
        currentHeroHealth = maxHeroHealth;
        rb = GetComponent<Rigidbody>();
        OnHealthChanged?.Invoke(currentHeroHealth, maxHeroHealth);
    }

    void Update()
    {
        RotateToMouse();

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveZ = 1f;
            if (Keyboard.current.sKey.isPressed) moveZ = -1f;
            if (Keyboard.current.aKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed) moveX = 1f;
        }

        if (Camera.main != null) 
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            movementInput = (camForward * moveZ + camRight * moveX).normalized;
        }
        else
        {
            movementInput = new Vector3(moveX, 0f, moveZ).normalized;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
            GameObject bullet = bulletPool.GetPooledObject();
            Vector3 spawnPosition = transform.position + transform.forward * 1.5f;
            Quaternion spawnRotation = transform.rotation;
            if (bullet != null)
            {
                bullet.transform.position = spawnPosition;
                bullet.transform.rotation = spawnRotation;
                bullet.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementInput.magnitude > 0f)
        {
            Vector3 targetPosition = rb.position + movementInput * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            speed = 0f;

            if (!isStuck)
            {
                currentHeroHealth = 0;
                Debug.Log($"Oh, no!!! {heroName} is stuck(999(99");
                isStuck = true;
                OnHealthChanged?.Invoke(currentHeroHealth, maxHeroHealth);
                OnHeroDied?.Invoke();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHeroHealth -= amount;
        OnHealthChanged?.Invoke(currentHeroHealth, maxHeroHealth);
            

        if (currentHeroHealth <= 0) 
        {
            speed = 0f;
            Debug.Log("Hero is dead((9");
            isStuck = true;
            OnHeroDied?.Invoke();
        }
    }

    private void RotateToMouse()
    {
        if (Mouse.current == null || Camera.main == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane (Vector3.up, transform.position);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude >0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
           
        }
    }
}
