using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Respawner.Instance.SetCheckpoint(transform);
            GetComponent<Collider>().enabled = false;
        }
    }
}