using UnityEngine;

public class ClearCounter : BaseCounter {

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // Player Drop object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else {
            // Player Pick up object
            if (!player.HasKitchenObject()) {
                KitchenObject.SetKitchenObjectParent(player);
            }
        }
    }
    
}