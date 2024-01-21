using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(Player player) {
        if (player.GetKitchenObject() == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        } else if (player.GetKitchenObject().GetKitchenObjectSO.ObjectName == this.kitchenObjectSO.ObjectName) {
            player.DestroyKitchenObject();
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}