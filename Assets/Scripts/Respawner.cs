using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
    public static Respawner Instance;

    [SerializeField] private Transform startingPoint;
    [SerializeField] private float respawnDelay = 1f;

    private Transform currentRespawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentRespawnPoint = startingPoint;
    }

    public void SetCheckpoint(Transform point)
    {
        currentRespawnPoint = point;
    }

    public void RespawnPlayer(PlayerMovement player)
    {
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(PlayerMovement player)
    {
        yield return new WaitForSeconds(respawnDelay);

        player.transform.position = currentRespawnPoint.position;
        player.Revive();

        // Reset all enemies
        foreach (var enemy in FindObjectsByType<FollowTarget>(FindObjectsSortMode.None))
        {
            enemy.ResetAgent();
        }
    }
}