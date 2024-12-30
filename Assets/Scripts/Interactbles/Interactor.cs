using UnityEngine;
using TMPro;

public class Interactor : MonoBehaviour
{
    public Transform cameraTransform;
    public float range = 3f;
    public float startOffset = 0.25f;
    public TextMeshProUGUI interactMessage;

    void Update()
    {
        CheckForInteractable();
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position + (cameraTransform.forward * startOffset), cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, range))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractables interactable))
            {
                interactMessage.text = interactable.InteractionMessage();
                interactMessage.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                interactMessage.enabled = false;
            }
        }
        else
        {
            interactMessage.enabled = false;
        }
    }
}