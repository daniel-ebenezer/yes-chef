using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigSO", menuName = "Scriptable Objects/PlayerConfigSO")]
public class PlayerConfigSO : ScriptableObject
{
    public float moveSpeed;
    public float interactableDistance;

    public LayerMask interactableLayerMask;

    public float rayHeight;
}
