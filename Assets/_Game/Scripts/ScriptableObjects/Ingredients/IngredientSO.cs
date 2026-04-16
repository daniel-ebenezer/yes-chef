using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Scriptable Objects/IngredientSO")]
public class IngredientSO : ScriptableObject
{
    [Header("Config")]
    public string ingredientName;

    public IngredientType type;

    public IngredientState initialState = IngredientState.Raw;
    public float preparationTime;

    public int baseScore;

    [Header("Visuals")]
    public GameObject visualPrefab;                 
    public Material rawMaterial;
    public Material preparedMaterial;

    [Header("UI Display")]
    public Sprite iconSprite;           
    public Color iconColor = Color.white;

    public bool RequiresPreparation => type == IngredientType.Vegetable || type == IngredientType.Meat;

    public IngredientState PreparedState =>
        type == IngredientType.Vegetable ? IngredientState.Chopped :
        type == IngredientType.Meat ? IngredientState.Cooked :
        IngredientState.Raw;

}

public enum IngredientType
{
    Vegetable,
    Meat,
    Cheese
}

public enum IngredientState
{
    Raw,
    Chopped,
    Cooked
}