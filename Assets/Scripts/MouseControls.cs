using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class MouseControls : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private PlayerMovement player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDeadPublic) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(ray, out RaycastHit hit, 50))
            {
                if (player.UseItem())
                {
                    Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                }
            }
        }
    }


}
