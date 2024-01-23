using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private GameObject sizzlingParticles;
    [SerializeField] private GameObject stoveOn;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool isOn = e.State == StoveCounter.State.Frying || e.State == StoveCounter.State.IsFried;
        sizzlingParticles.SetActive(isOn);
        stoveOn.SetActive(isOn);
    }
}