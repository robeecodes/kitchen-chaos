using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private Image backgroundBar;

    private IHasProgress _hasProgress;
    private BaseCounter _baseCounter;

    private Dictionary<string, Color32> _colours;

    private void Awake() {
        _colours = new Dictionary<string, Color32> {
            { "inProgress", new Color32(219, 154, 32, 255) },
            { "complete", new Color32(70, 207, 34, 255) },
            { "overdone", new Color32(207, 24, 32, 255) }
        };
    }

    private void Start() {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        _baseCounter = hasProgressGameObject.GetComponent<BaseCounter>();

        if (_hasProgress == null) {
            Debug.LogError("Game object " + hasProgressGameObject + " does not implement IHasProgress.");
        }

        if (_baseCounter == null) {
            Debug.LogError("Game object " + hasProgressGameObject + " does not have a BaseCounter.");
        }
        
        _hasProgress!.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
    }

    private void Update() {
        backgroundBar.enabled = _baseCounter.HasKitchenObject();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.ProgressNormalised;
        barImage.color = barImage.fillAmount == 1f ? _colours["complete"] : _colours["inProgress"];
        if (_baseCounter.GetType() == typeof(StoveCounter) ) {
            StoveCounter stove = (StoveCounter) _baseCounter;
            if (stove.GetState() == StoveCounter.State.IsFried) {
                barImage.color = _colours["complete"];
            }
            if (stove.GetState() == StoveCounter.State.IsBurnt) {
                barImage.color = _colours["overdone"];
            }
        }
    }
}