using UnityEngine;
using System.Threading;

public class BaseStation : MonoBehaviour, IInteractable
{
    [SerializeField] string defaultPrompt = "Interact";
    
    public virtual bool CanInteract(PlayerInteraction player) => true;

    public virtual string GetInteractionPrompt() => defaultPrompt;
    
    public virtual void Interact(PlayerInteraction player)
    {
        
    }

    protected CancellationToken DestroyToken => destroyCancellationToken; // ?
}
