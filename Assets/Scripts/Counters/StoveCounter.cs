using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    [SerializeField] private FryingRecipeSO[] fryingRecipes;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State State;
    }
    
    public enum State {
        Idle,
        Frying,
        IsFried,
        IsBurnt
    }

    private FryingRecipeSO _fryingRecipeSO;
    private float _fryingTimer;
    private float _burningTimer;
    private State _state = State.Idle;

    private void Start() {
        _state = State.Idle;
        ChangeStateEvent();
    }

    private void Update() {
        // Only count if there is an object on the stove
        if (HasKitchenObject()) {
            switch (_state) {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    if (_fryingTimer > _fryingRecipeSO.fryingTimerMax) {
                        // Cooking Complete
                        _burningTimer = 0f;

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                        _fryingRecipeSO = GetFryingRecipeSO(_fryingRecipeSO.output);
                        _state = State.IsFried;
                        ChangeStateEvent();
                    }
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
                        ProgressNormalised = _fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    break;
                case State.IsFried:
                    _burningTimer += Time.deltaTime;
                    if (_burningTimer > _fryingRecipeSO.fryingTimerMax) {
                        // Burnt food
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);

                        _state = State.IsBurnt;
                        ChangeStateEvent();
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
                            ProgressNormalised = 0f
                        });
                    }
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
                        ProgressNormalised = _burningTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    break;
                case State.IsBurnt:
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if (_state == State.Idle) {
            if (player.HasKitchenObject()) {
                // Player drop object if valid for frying
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // Initialise recipe
                    _fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                    _fryingTimer = 0f;
                    _state = State.Frying;
                    ChangeStateEvent();
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
                        ProgressNormalised = _fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        }
        else if (HasKitchenObject() && _state != State.Frying) {
            GetKitchenObject().SetKitchenObjectParent(player);
            _fryingRecipeSO = null;
            _state = State.Idle;
            ChangeStateEvent();
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
                ProgressNormalised = 0f
            });
        }
    }

    public State GetState() {
        return _state;
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO input) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipes) {
            if (fryingRecipeSO.input == input) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO input) {
        // Check if a recipe exists for ingredient on board
        return GetFryingRecipeSO(input) != null;
    }

    private void ChangeStateEvent() {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() {
            State = _state
        });
    }
}