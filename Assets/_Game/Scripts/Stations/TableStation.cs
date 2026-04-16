using UnityEngine;

public class TableStation : BaseStation
{
    [Header("Chopping Table")]
    [SerializeField] private float chopTime = 2f;
    [SerializeField] private ProgressBarUI progressBar;

    Ingredient currentIngredientOnTable;
    Awaitable currentChopTask;

    void Start()
    {
        progressBar.Hide();
    }

    public override string GetInteractionPrompt()
    {
        if (currentIngredientOnTable == null)
            return "Place Vegetable to Chop";

        return currentIngredientOnTable.IsPrepared 
            ? "Pick up Chopped Vegetable" 
            : "Chop Vegetable";
    }

    public override bool CanInteract(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
        {
            Ingredient heldIngredient = player.CurrentHeldIngredient;
            return heldIngredient.data.type == IngredientType.Vegetable && !heldIngredient.IsPrepared && currentIngredientOnTable == null;
        }
        else
        {
            return currentIngredientOnTable != null && currentIngredientOnTable.IsPrepared;
        }
    }

    public override void Interact(PlayerInteraction player)
    {
        if (player.IsHoldingItem)
        {
            currentIngredientOnTable = player.GetAndClearHeldItem();
            currentChopTask = ChopVegetableAsync();

            if (progressBar != null)
            {
                progressBar.Show();
                progressBar.ResetBar();
            }
        }
        else if (currentIngredientOnTable != null && currentIngredientOnTable.IsPrepared)
        {
            player.TryPickUp(currentIngredientOnTable);
            ClearTable();
        }
    }

    private async Awaitable ChopVegetableAsync()
    {
        float timer = 0f;

        try
        {
            while (timer < chopTime)
            {
                timer += Time.deltaTime;
                float progress = timer / chopTime;

                if (progressBar != null)
                    progressBar.SetProgress(progress);
                await Awaitable.NextFrameAsync();
            }

            if (currentIngredientOnTable != null)
                currentIngredientOnTable.currentState = IngredientState.Chopped;
        }
        catch (System.OperationCanceledException) { }
    }

    void ClearTable()
    {
        currentIngredientOnTable = null;
        currentChopTask?.Cancel();
        currentChopTask = null;

        if (progressBar != null)
            progressBar.Hide();
    }

    void OnDestroy()
    {
        currentChopTask?.Cancel();
    }
}