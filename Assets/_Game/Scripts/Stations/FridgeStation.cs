using UnityEngine;

public class FridgeStation : BaseStation
{
    [Header("Available Ingredients")]
    [SerializeField] private IngredientSO vegetableSO;
    [SerializeField] private IngredientSO cheeseSO;
    [SerializeField] private IngredientSO meatSO;

    public bool isUIOpen = false;

    public GameObject uiPanel;

    public override string GetInteractionPrompt()
    {
        return isUIOpen ? "" : "Open Fridge";
    }

    public override bool CanInteract(PlayerInteraction player)
    {
        return !player.IsHoldingItem && !isUIOpen;
    }

    public override void Interact(PlayerInteraction player)
    {
        if (!player.IsHoldingItem)
        {
            OpenFridgeUI();
        }
    }

    void OpenFridgeUI()
    {
        isUIOpen = true;
        uiPanel.SetActive(true);
    }

    public void SelectVegetable(PlayerInteraction player) => GiveIngredient(vegetableSO, player);
    public void SelectCheese(PlayerInteraction player)    => GiveIngredient(cheeseSO, player);
    public void SelectMeat(PlayerInteraction player)      => GiveIngredient(meatSO, player);

    void GiveIngredient(IngredientSO ingredientSO, PlayerInteraction player)
    {
        if (ingredientSO == null) return;

        Ingredient newIngredient = new Ingredient(ingredientSO);

        bool hasTakenIngredient  = player.TryPickUp(newIngredient);

        CloseFridgeUI();
    }

    public void CloseFridgeUI()
    {
        isUIOpen = false;
        uiPanel.SetActive(false);
    }
}