using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Rendering")]
    public SpriteRenderer sprRenderer;
    public Material defaultMaterial;
    public Material onHitMaterial;
    public int hitFrames;

    public void OnHit()
    {
        StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        sprRenderer.material = onHitMaterial;
        for (int i = 0; i < hitFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        sprRenderer.material = defaultMaterial;
    }

}
