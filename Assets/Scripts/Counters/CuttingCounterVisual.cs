using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {
    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator _animator;
    private static readonly int Cut = Animator.StringToHash("Cut");

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
    }

    private void CuttingCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        // Don't cut if already chopped
        if (e.ProgressNormalised != 0f) {
            _animator.SetTrigger(Cut);
        }
    }
}