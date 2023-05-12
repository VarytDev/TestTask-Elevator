using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer = null;

    [Header("Interaction Visualization")]
    [SerializeField] private Material defaultMateraial = null;
    [SerializeField] private Material selectedMateraial = null;

    [Header("Interaction Settings")]
    [SerializeField] private UnityEvent onInteractionEvents = null;

    private void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    public void OnStartInteractableHover()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        meshRenderer.material = selectedMateraial;
    }

    public void OnEndInteractableHover()
    {
        if (isAnyRequiredComponentNull() == true)
        {
            return;
        }

        meshRenderer.material = defaultMateraial;
    }

    public void OnInteractableAction()
    {
        onInteractionEvents.Invoke();
    }

    private bool isAnyRequiredComponentNull()
    {
        if (meshRenderer == null || defaultMateraial == null || selectedMateraial == null)
        {
            Debug.LogError("InteractableButton :: One of required components is null!", this);
            return true;
        }

        return false;
    }
}
