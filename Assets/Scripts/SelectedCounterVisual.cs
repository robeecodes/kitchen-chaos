using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] visualCounters;

    private void Start() {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeArgs e) {
        if (e.SelectedCounter == counter) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject visualCounter in visualCounters) {
            visualCounter.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualCounter in visualCounters) {
            visualCounter.SetActive(false);
        }
    }
}