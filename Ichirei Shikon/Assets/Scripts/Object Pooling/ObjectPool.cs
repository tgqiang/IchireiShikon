using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool : MonoBehaviour {

    [SerializeField]
    protected Transform parentTransform;
    [SerializeField]
    protected int quantity;
    [SerializeField]
    protected GameObject[] prefabs;


	protected virtual void Start () {
        AssertRequiredConditions();
	}

    protected virtual void AssertRequiredConditions () {
        Debug.Assert(quantity >= Configurable.instance.MIN_OBJECT_POOL_QUANTITY, "Likely insufficient quantity specified for ObjectPool, currently set at " + Configurable.instance.MIN_OBJECT_POOL_QUANTITY);
        Debug.Assert(prefabs != null, "Required prefabs for ObjectPool script not initialized.");
        Debug.Assert(prefabs.Length > 0, "Prefabs for ObjectPool script not assigned for performing object-pooling.");
    }
	
}
