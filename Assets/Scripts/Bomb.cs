using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private AudioClip explosionSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        // If it hit an enemy, kill it
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FollowTarget enemy = collision.gameObject.GetComponent<FollowTarget>();

            if (enemy != null)
            {
                enemy.Die();
            }
        }

        // Always explode no matter what we hit
        Explode();
    }

    private void Explode()
    {
        // Spawn VFX
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // Play sound in world
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Destroy bomb
        Destroy(gameObject);
    }



}
