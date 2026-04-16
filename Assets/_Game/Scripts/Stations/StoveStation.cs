

using UnityEngine;

public class StoveStation : BaseStation
{
    [Header("Stove")]
    [SerializeField] private float cookTime = 6f;
    [SerializeField] private ProgressBarUI[] slotProgressBars = new ProgressBarUI[2];

    private class StoveSlot
    {
        public Ingredient meat;
        public Awaitable cookTask;
        public bool isCooking = false;
    }

    StoveSlot[] slots = new StoveSlot[2];

    void Awake()
    {
        for (int i = 0; i < 2; i++)
            slots[i] = new StoveSlot();
    }

    void Start()
    {
        foreach (var bar in slotProgressBars)
            bar.Hide();
    }

    public override string GetInteractionPrompt()
    {
        return HasCookedMeatReady() ? "Take Cooked Meat" : "Place Raw Meat";
    }

    public override bool CanInteract(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
        {
            Ingredient heldIngredient = player.CurrentHeldIngredient;
            return heldIngredient.data.type == IngredientType.Meat && !heldIngredient.IsPrepared;
        }
        else
        {
            return HasCookedMeatReady();
        }
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
            TryPlaceMeat(player);
        else
            TryTakeCookedMeat(player);
    }

    bool TryPlaceMeat(PlayerInteraction player)
    {
        for (int i = 0; i < 2; i++)
        {
            if (slots[i].meat == null)   
            {
                slots[i].meat = player.GetAndClearHeldItem();
                slots[i].isCooking = true;

                if (slotProgressBars[i] != null)
                {
                    slotProgressBars[i].Show();
                    slotProgressBars[i].ResetBar();
                }

                slots[i].cookTask = CookSlotAsync(i);
                return true;
            }
        }

        // Debug.Log("Stove full!"); // make this interaction prompt?
        return false;
    }

    bool TryTakeCookedMeat(PlayerInteraction player)
    {
        for (int i = 0; i < 2; i++)
        {
            if (slots[i].meat != null && slots[i].meat.IsPrepared)
            {
                player.TryPickUp(slots[i].meat);

                slots[i].meat = null;
                slots[i].isCooking = false;
                slots[i].cookTask?.Cancel();
                slots[i].cookTask = null;

                if (slotProgressBars[i] != null)
                    slotProgressBars[i].Hide();

                return true;
            }
        }
        return false;
    }

    private async Awaitable CookSlotAsync(int slotIndex)
    {
        float timer = 0f;

        try
        {
            while (timer < cookTime && slots[slotIndex].isCooking)
            {
                timer += Time.deltaTime;
                float progress = timer / cookTime;

                if (slotProgressBars[slotIndex] != null)
                    slotProgressBars[slotIndex].SetProgress(progress);

                await Awaitable.NextFrameAsync();
            }

            if (slots[slotIndex].meat != null)
            {
                slots[slotIndex].meat.currentState = IngredientState.Cooked;
                slots[slotIndex].isCooking = false;
            }
        }
        catch (System.OperationCanceledException) { }
    }

    bool HasCookedMeatReady()
    {
        foreach (var slot in slots)
        {
            if (slot.meat != null && slot.meat.IsPrepared)
                return true;
        }
        return false;
    }

    void OnDestroy()
    {
        for (int i = 0; i < 2; i++)
        {
            if (slots[i].cookTask != null)
                slots[i].cookTask.Cancel();
        }
    }
}