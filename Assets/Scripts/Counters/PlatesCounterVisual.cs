using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {
    [SerializeField] private Transform counterTop;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> _plateVisuals;

    private void Awake() {
        _plateVisuals = new List<GameObject>();
    }

    private void Start() {
        platesCounter.OnPlateSpawn += PlatesCounter_OnPlateSpawn;
        platesCounter.OnPlateTaken += PlatesCounter_OnPlateTaken;
    }

    private void PlatesCounter_OnPlateTaken(object sender, EventArgs e) {
        Destroy(_plateVisuals.Last().gameObject);
        _plateVisuals.Remove(_plateVisuals.Last());
    }

    private void PlatesCounter_OnPlateSpawn(object sender, EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTop);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _plateVisuals.Count, 0);
        
        _plateVisuals.Add(plateVisualTransform.gameObject);
    }
}