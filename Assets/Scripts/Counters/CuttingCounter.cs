using System;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs {
        public float progressNormalised;
    }
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private CuttingRecipeSO _cuttingRecipeSO;
    private int _cuttingProgress = 0;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // Player drop object if valid for slicing
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    
                    // Get the cutting recipe
                    _cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                }
            }
        }
        else {
            // Player Pick up object
            if (!player.HasKitchenObject()) {
                KitchenObject.SetKitchenObjectParent(player);
                
                // Remove cutting recipe
                _cuttingRecipeSO = null;
                
                _cuttingProgress = 0;
                    
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs() {
                    progressNormalised = 0f
                });
            }
        }
    }

    public override void InteractAlternate(Player player) {
        // Chop the ingredient if it exists and isn't fully chopped
        if (_cuttingRecipeSO != null && _cuttingProgress < _cuttingRecipeSO.cuttingProgressMax) {
            _cuttingProgress++;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs() {
                progressNormalised = (float) _cuttingProgress / _cuttingRecipeSO.cuttingProgressMax
            });
            if (_cuttingProgress == _cuttingRecipeSO.cuttingProgressMax) {
                // When fully chopped, replace ingredient with slices
                GetKitchenObject().DestroySelf();
                this.KitchenObject = KitchenObject.SpawnKitchenObject(_cuttingRecipeSO.output, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO input) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOs) {
            if (cuttingRecipeSO.input == input) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO input) {
        // Check if a recipe exists for ingredient on board
        return GetCuttingRecipeSO(input) != null;
    }
}