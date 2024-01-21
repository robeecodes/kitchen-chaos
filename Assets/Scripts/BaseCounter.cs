using UnityEngine;

public abstract class BaseCounter : MonoBehaviour {
    public virtual void Interact(Player player) {
        // if (_kitchenObject == null) {
        //     Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab, counterTop);
        //     kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        // }
        // else {
        //     _kitchenObject.SetKitchenObjectParent(player);
        // }
    }
    
}