using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    private IKitchenObjectParent _kitchenObjectParent;
    
    public KitchenObjectSO GetKitchenObjectSO => _kitchenObjectSO;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        this._kitchenObjectParent?.ClearKitchenObject();
        this._kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("kitchenObjectParent has an object!! 😱😱");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return _kitchenObjectParent;
    }
}
