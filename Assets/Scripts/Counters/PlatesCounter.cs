using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateTaken;
    
    [SerializeField] private KitchenObjectSO plateSO;
    
    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _platesSpawned;
    private int _maxPlatesSpawned = 4;
    
    // Only 4 plates will appear by spawn, but you can stack up to 6
    private int _absoluteMaxPlatesSpawned = 6;

    private void Update() {
        _spawnPlateTimer += Time.deltaTime;

        if (_spawnPlateTimer > _spawnPlateTimerMax) {
            _spawnPlateTimer = 0f;

            if (_platesSpawned < _maxPlatesSpawned) {
                _platesSpawned++;
                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (_platesSpawned > 0 && !player.HasKitchenObject()) {
            // If a plate is available give to empty-handed player
            KitchenObject.SpawnKitchenObject(plateSO, player);
            _platesSpawned--;
            OnPlateTaken?.Invoke(this, EventArgs.Empty);
        } else if (player.GetKitchenObject().GetKitchenObjectSO().ObjectName == this.plateSO.ObjectName) {

            if (_platesSpawned < _absoluteMaxPlatesSpawned) {
                // Return plate as long as there are less than 6
                player.GetKitchenObject().DestroySelf();
                _platesSpawned++;
                OnPlateSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}