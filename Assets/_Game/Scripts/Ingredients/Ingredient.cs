using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public IngredientSO data { get; private set; }
    public IngredientState currentState { get; set; }

    public Ingredient(IngredientSO config)
    {
        data = config;
        currentState = data.initialState;
    }

    public bool IsPrepared =>
        !data.RequiresPreparation || currentState == data.PreparedState;

    public int ScoreValue => data.baseScore;
    public void ApplyVisualState(GameObject visualObject)
    {
        if (visualObject == null) return;

        MeshRenderer renderer = visualObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = IsPrepared ? data.preparedMaterial : data.rawMaterial;
        }
    }
}