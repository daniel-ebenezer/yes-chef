    using System.Runtime.CompilerServices;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerConfigSO playerConfig;
        [SerializeField] private Transform holdPoint;
        
        [SerializeField] private EventChannelSO<string> interactionPromptChannel;

        Ingredient currentHeldIngredient;
        GameObject currentHeldVisual;

        IInteractable currentLookedAt;
        public Ingredient CurrentHeldIngredient => currentHeldIngredient;

        public bool IsHoldingItem => currentHeldIngredient != null;
        Ray interactionRay;
        public void OnInteracted(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) // check ?
                return;
            currentLookedAt?.Interact(this);
        }

        void Update()
        {
            UpdateInteractionRay();
            LookForInteractable();
        }

        void UpdateInteractionRay()
        {
            interactionRay.origin = transform.position + Vector3.up * playerConfig.rayHeight;
            interactionRay.direction = transform.forward;  
        }

        void LookForInteractable()
        {
            if(Physics.Raycast(interactionRay, out RaycastHit hit, playerConfig.interactableDistance, playerConfig.interactableLayerMask))
            {
                if(hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (interactable.CanInteract(this))
                    {
                        if (currentLookedAt != interactable )
                        {
                            currentLookedAt = interactable;
                            //OnInteractionPromptChanged?.Invoke(interactable.GetInteractionPrompt());
                            interactionPromptChannel?.Raise(interactable.GetInteractionPrompt());
                        }
                        return;
                    }
                }
            }
            else
            {
                currentLookedAt = null;
                //OnInteractionPromptChanged?.Invoke(string.Empty);
                interactionPromptChannel?.Raise("");
            }
        }

        public bool TryPickUp(Ingredient ingredient)
        {
            if (IsHoldingItem || ingredient == null) return false;

            currentHeldIngredient = ingredient;

            if (ingredient.data.visualPrefab != null && holdPoint != null)
            {
                currentHeldVisual = Instantiate(ingredient.data.visualPrefab, holdPoint);
                currentHeldVisual.transform.localPosition = Vector3.zero;
                currentHeldVisual.transform.localRotation = Quaternion.identity;

                ingredient.ApplyVisualState(currentHeldVisual);
            }

            return true;
        }

        public Ingredient GetAndClearHeldItem()
        {
            Ingredient item = currentHeldIngredient;
            ClearHeldItem();
            return item;
        }

        public void ClearHeldItem()
        {
            if (currentHeldVisual != null)
            {
                Destroy(currentHeldVisual);
                currentHeldVisual = null;
            }
            currentHeldIngredient = null;
        }
    }

