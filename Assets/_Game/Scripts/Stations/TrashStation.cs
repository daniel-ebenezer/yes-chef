using UnityEngine;

public class TrashStation : BaseStation
{
    public override bool CanInteract(PlayerInteraction player)
    {
        return player.IsHoldingItem;
    }

    public override string GetInteractionPrompt()
    {
        return "Throw away ingredient!";
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
        {
            player.ClearHeldItem();
        }
    }
}
