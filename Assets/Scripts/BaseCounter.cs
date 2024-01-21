using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] protected KitchenObjectSO kitchenObjectSO;
    [SerializeField] protected Transform counterTop;
    protected KitchenObject KitchenObject;

    public virtual void Interact(Player player) {
        if (KitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab, counterTop);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else {
            KitchenObject.SetKitchenObjectParent(player);
        }
    }

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

    public void DestroyKitchenObject() {
        Destroy(KitchenObject.gameObject);
        ClearKitchenObject();
    }

    public bool HasKitchenObject() {
        return KitchenObject != null;
    }
}