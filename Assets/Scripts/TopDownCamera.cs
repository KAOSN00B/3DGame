using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, -10);
    [SerializeField] private float followSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Position behind player relative to facing direction
        Vector3 desiredPosition = target.position
                                  + target.forward * offset.z
                                  + Vector3.up * offset.y;

        transform.position = Vector3.Lerp(transform.position,
                                          desiredPosition,
                                          followSpeed * Time.deltaTime);

        transform.LookAt(target);
    }

    public void SnapToTarget()
    {
        Vector3 desiredPosition = target.position
                                  + target.forward * offset.z
                                  + Vector3.up * offset.y;

        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
