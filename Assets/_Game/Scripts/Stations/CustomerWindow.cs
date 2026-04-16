using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerWindow : BaseStation
{
    [Header("Customer Window")]
    public int windowIndex;

    [Header("UI References - Order Display")]
    [SerializeField] private Transform ingredientsContainer;        
    [SerializeField] private Image ingredientIconPrefab;  

    [SerializeField] private TMP_Text scoreValue;          

    Order currentOrder;
    List<Image> currentIngredientIcons = new List<Image>();

    [Header("Manager Reference")]
    [SerializeField] private OrderManager orderManager; // make this an event 

    public Order CurrentOrder => currentOrder;

    public override string GetInteractionPrompt()
    {
        return currentOrder != null ? "Deliver Prepared Ingredient" : "Waiting for Order!";
    }

    public override bool CanInteract(PlayerInteraction player)
    {
        if (currentOrder == null || !player.IsHoldingItem)
            return false;

        return player.CurrentHeldIngredient.IsPrepared;
    }

    public override void Interact(PlayerInteraction player)
    {
        if (currentOrder == null || !player.IsHoldingItem) return;

        Ingredient heldIngredient = player.GetAndClearHeldItem();
        //Debug.Log(held.ScoreValue);

        bool wasNeeded = currentOrder.TryDeliverIngredient(heldIngredient);

        if (wasNeeded)
        {
            UpdateIngredientDisplay();        

            if (currentOrder.IsComplete)
            {
                CompleteOrder();
            }
        }
        else
        {
            player.TryPickUp(heldIngredient); 
        }
    }

    public void AssignOrder(Order newOrder)
    {
        currentOrder = newOrder;
        UpdateIngredientDisplay();
    }

    public void ClearOrder()
    {
        currentOrder = null;
        ClearIngredientIcons();
    }

    void CompleteOrder()
    {
        int score = currentOrder.CalculateScore();
        scoreValue.text = score >= 0 ? $"+{score}" : score.ToString();
        scoreValue.GetComponent<Animation>().Play("ScoreFade");
        orderManager?.OnOrderCompleted(windowIndex, score); //to decouple
        ClearOrder();
    }

    // void Update()
    // {
    //     if (currentOrder != null && timerText != null)
    //     {
    //         timerText.text = $"{currentOrder.TimeSinceSpawned:F1}s";
    //     }
    // }


    void UpdateIngredientDisplay()
    {
        ClearIngredientIcons();

        if (currentOrder == null) return;

        foreach (var ingredientSO in currentOrder.GetRemainingIngredients())
        {
            Image icon = Instantiate(ingredientIconPrefab, ingredientsContainer);
            icon.sprite = ingredientSO.iconSprite;
            icon.color = ingredientSO.iconColor;
            currentIngredientIcons.Add(icon);
        }
    }

    void ClearIngredientIcons()
    {
        foreach (var icon in currentIngredientIcons)
        {
            if (icon != null) Destroy(icon.gameObject);
        }
        currentIngredientIcons.Clear();
    }

}