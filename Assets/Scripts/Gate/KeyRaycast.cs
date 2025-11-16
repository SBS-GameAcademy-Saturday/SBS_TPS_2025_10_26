using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRaycast : MonoBehaviour
{
    [Header("RayCast Radius and Layer")]
    [SerializeField] private int rayDistance = 6;
    [SerializeField] private LayerMask collectiveLayerMask;
    [SerializeField] private MainHud mainHud;

    private KeyObject keyObject;
    private KeyGate keyGate;
    private MainComputer mainComputer;
    private GeneratorComputer generatorComputer;
    private EscapeCar escapeCar;

    private void LateUpdate()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, transform.forward,
            out hitInfo, rayDistance, collectiveLayerMask))
        {
            if (hitInfo.collider.TryGetComponent<KeyObject>(out KeyObject key))
            {
                keyObject = key;
                mainHud.CanCollectiveState(true);
            }
            if(hitInfo.collider.TryGetComponent<KeyGate>(out KeyGate gate))
            {
                keyGate = gate;
                mainHud.CanCollectiveState(true);
            }
            if(hitInfo.collider.TryGetComponent<MainComputer>(out MainComputer main))
            {
                mainComputer = main;
                mainHud.CanCollectiveState(true);
            }
            if (hitInfo.collider.TryGetComponent<GeneratorComputer>(out GeneratorComputer generator))
            {
                generatorComputer = generator;
                mainHud.CanCollectiveState(true);
            }
            if (hitInfo.collider.TryGetComponent<EscapeCar>(out EscapeCar car))
            {
                escapeCar = car;
                mainHud.CanCollectiveState(true);
            }
        }
        else
        {
            mainHud.CanCollectiveState(false);
            keyObject = null;
            keyGate = null;
            mainComputer = null;
            generatorComputer = null;
            escapeCar = null;
        }
    }

    public void OnInteractAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (keyObject != null)
            {
                keyObject.FoundKey();
                mainHud.CanCollectiveState(false);
            }
            if(keyGate != null)
            {
                keyGate.ToggleGate();
                mainHud.CanCollectiveState(false);
            }
            if (mainComputer != null)
            {
                mainComputer.ToggleComputer();
            }
            if (generatorComputer != null)
            {
                generatorComputer.ToggleComputer();
            }
            if(escapeCar != null)
            {
                escapeCar.EscapeFactory();
            }
        }
    }
}
