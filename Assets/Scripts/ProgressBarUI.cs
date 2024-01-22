using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;
    [SerializeField] private Image backgroundBar;

    private void Start() {
        backgroundBar.enabled = false;
        barImage.fillAmount = 0f;
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
    }

    private void Update() {
        backgroundBar.enabled = cuttingCounter.HasKitchenObject();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalised;
        barImage.color = barImage.fillAmount > 0.95f ? new Color32(70, 207, 34, 255) : new Color32(219, 154, 32, 255);
    }
}