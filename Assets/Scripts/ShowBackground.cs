using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBackground : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }
}
