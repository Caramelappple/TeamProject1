using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [SerializeField] private LayerMask defaultLayer;

    private ContactFilter2D filter;
    private readonly Collider2D[] results = new Collider2D[10];
    private readonly HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    private void Awake()
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(defaultLayer);
        filter.useTriggers = true;
    }

    public void ResetHit()
    {
        hitTargets.Clear();
    }


    public void Cast(Collider2D hitBox, System.Action<Collider2D> onHit)
    {
        Cast(hitBox, onHit, defaultLayer);
    }
    public void Cast(Collider2D hitBox, System.Action<Collider2D> onHit, LayerMask layer)
    {
        if (hitBox == null) return;

        ContactFilter2D tempFilter = filter;
        tempFilter.SetLayerMask(layer);

        //
        int count = hitBox.Overlap(tempFilter, results);

        for (int i = 0; i < count; i++)
        {
            var target = results[i];
            if (!target || hitTargets.Contains(target)) continue;

            hitTargets.Add(target);

            onHit?.Invoke(target);

            results[i] = null;
        }
    }
}
