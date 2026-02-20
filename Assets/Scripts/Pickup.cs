using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.PickUpItem();
                Destroy(gameObject);
            }
        }
    }

}
