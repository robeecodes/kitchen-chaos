using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualCounter;

    private void Start() {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeArgs e) {
        if (e.SelectedCounter == clearCounter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        visualCounter.SetActive(true);
    }

    private void Hide() {
        visualCounter.SetActive(false);
    }
}