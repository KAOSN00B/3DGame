using UnityEngine;

public class AISight : MonoBehaviour
{
    [SerializeField] private GameObject aiHead;
    [SerializeField] private FollowTarget aiBehaviour;
    [SerializeField] private LayerMask layerMask;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        RaycastHit hit;

        if (Physics.Linecast(aiHead.transform.position,
            other.transform.position, out hit, layerMask))
        {
            if (hit.transform == other.transform)
            {
 
                Debug.Log($"Line Casted to {hit.transform.name}");
                Debug.DrawLine(aiHead.transform.position,
                    other.transform.position,Color.red,2f);

                aiBehaviour.FoundHostile(other.transform);
            }
            else
            {

                Debug.DrawLine(aiHead.transform.position, 
                    hit.point,Color.yellow,2f);
            }
        }
    }

}
