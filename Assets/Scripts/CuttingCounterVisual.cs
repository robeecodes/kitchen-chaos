using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {
    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator _animator;
    private static readonly int Cut = Animator.StringToHash("Cut");

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnProgressChanged += CuttingCounterOnOnProgressChanged;
    }

    private void CuttingCounterOnOnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e) {
        // Don't cut if already chopped
        if (e.progressNormalised != 0f) {
            _animator.SetTrigger(Cut);
        }
    }
}