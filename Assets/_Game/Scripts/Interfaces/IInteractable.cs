using UnityEngine;

public interface IInteractable
{
    bool CanInteract(PlayerInteraction player);
    void Interact(PlayerInteraction player);
    string GetInteractionPrompt();  

}
