using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomRenderer : MonoBehaviour
{
    public int variantCount;

    private void Start()
    {
        GetComponent<Animator>().SetInteger("variant", Random.Range(0, variantCount));
    }
}
