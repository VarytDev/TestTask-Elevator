using UnityEngine;

public class InteractionFinder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera = null;

    [Header("Interaction Ray Settings")]
    [SerializeField] private LayerMask interactionRayLayerMask = 1 << 6; //Interactable layer
    [SerializeField] private float interactableRayLength = 3;

    private IInteractable currentInteractable = null;

    private void Start()
    {
        if(playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        Ray _ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(_ray, out RaycastHit _raycastHit, interactableRayLength, interactionRayLayerMask))
        {
            if (_raycastHit.DoesImplementInterface(out IInteractable _foundInteractable) == true)
            {
                handleFoundInteractable(_foundInteractable);

                if (Input.GetMouseButtonDown(0)) //TODO Convert to new input system
                {
                    currentInteractable.OnInteractableAction();
                }
            }
        }
        else if (currentInteractable != null)
        {
            handleExitingInteractable();
        }
    }

    private void handleFoundInteractable(IInteractable _foundInteractable)
    {
        if (currentInteractable == null)
        {
            currentInteractable = _foundInteractable;
            currentInteractable.OnStartInteractableHover();
        }
        else if (currentInteractable != _foundInteractable)
        {
            currentInteractable.OnEndInteractableHover();
            currentInteractable = _foundInteractable;
            currentInteractable.OnStartInteractableHover();
        }
    }

    private void handleExitingInteractable()
    {
        currentInteractable.OnEndInteractableHover();
        currentInteractable = null;
    }

    private bool isAnyRequiredComponentNull()
    {
        if (playerCamera == null)
        {
            Debug.LogError("InteractionFinder :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
