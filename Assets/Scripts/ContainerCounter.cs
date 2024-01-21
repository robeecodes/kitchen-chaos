using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTop;

    private KitchenObject _kitchenObject;

    public override void Interact(Player player) {
        if (_kitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab, counterTop);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else {
            // TODO: Give object to player
            _kitchenObject.SetKitchenObjectParent(player);
        }
    }
    
    public Transform GetKitchenObjectFollowTransform() {
        return counterTop;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return _kitchenObject;
    }

    public void ClearKitchenObject() {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return _kitchenObject != null;
    }
}