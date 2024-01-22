using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] protected Transform counterTop;
    protected KitchenObject KitchenObject;

    public virtual void Interact(Player player) {
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

    public virtual void InteractAlternate(Player player) { }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTop;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.KitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return KitchenObject;
    }

    public void ClearKitchenObject() {
        KitchenObject = null;
    }

    public bool HasKitchenObject() {
        return KitchenObject != null;
    }
}