using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] protected KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(Player player) {
        if (!player.GetKitchenObject()) {
            // Player is not carrying anything
            this.KitchenObject = KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        } else if (player.GetKitchenObject().GetKitchenObjectSO().ObjectName == this.kitchenObjectSO.ObjectName) {
            // Player is carrying the same object type contained in the counter
            player.GetKitchenObject().DestroySelf();
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}