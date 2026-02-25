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

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 clickPoint = ray.GetPoint(distance);

                if (player.UseItem())
                {
                    Instantiate(objectToSpawn, clickPoint, Quaternion.identity);
                }
            }
        }
    }


}
