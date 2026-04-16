using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDatabaseSO", menuName = "Scriptable Objects/IngredientDatabaseSO")]
public class IngredientDatabaseSO : ScriptableObject
{
    [SerializeField] List<IngredientSO> availableIngredients = new List<IngredientSO>();    

    public IngredientSO GetRandomIngredient()
    {
        if (availableIngredients.Count == 0)
        {
            return null;
        }
        return availableIngredients[Random.Range(0,availableIngredients.Count)];
    }
    
}
