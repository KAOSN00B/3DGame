using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
    public static Respawner Instance;

    [Header("Respawn")]
    [SerializeField] private Transform startingPoint;
    [SerializeField] private float respawnDelay = 1f;

    private Transform currentRespawnPoint;

    private void Awake()
    {
        // Basic singleton pattern
        if (Instance != null && Instance != this)
        {
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentRespawnPoint = startingPoint;
    }

    public void SetCheckpoint(Transform point)
    {
        if (point == null) return;
        currentRespawnPoint = point;
    }

    public void RespawnPlayer(PlayerMovement player)
    {
        if (player == null) return;
        StopAllCoroutines();
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(PlayerMovement player)
    {
        yield return new WaitForSeconds(respawnDelay);

        // Teleport player safely (use rb if you want)
        player.transform.SetPositionAndRotation(currentRespawnPoint.position, currentRespawnPoint.rotation);
        player.Revive();

        // Reset ONLY enemies (by layer)
        foreach (var enemy in FindObjectsByType<FollowTarget>(FindObjectsSortMode.None))
        {
            if (enemy == null) continue;

            if (enemy.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;

            enemy.ResetAgent();
            enemy.RespawnBehindPlayer(player.transform);
        }
    }
}