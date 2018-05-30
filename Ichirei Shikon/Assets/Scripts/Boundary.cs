using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    public void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer != LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.DEFAULT])) {
            other.gameObject.SetActive(false);
        }
    }

    public void OnTriggerExit2D (Collider2D other) {
        if (other.gameObject.layer != LayerMask.NameToLayer(Configurable.instance.LAYER_NAMES[(int) Configurable.LayerNameIndices.DEFAULT])) {
            other.gameObject.SetActive(true);
        }
    }
}
