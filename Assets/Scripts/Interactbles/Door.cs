using UnityEngine;

public class Door : MonoBehaviour, IInteractables
{
    public string InteractionMessage()
    {
        return "Press E to open the door";
    }

    public void Interact()
    {
        Debug.Log("The door opens!");
        // door opening logic here
    }
}
