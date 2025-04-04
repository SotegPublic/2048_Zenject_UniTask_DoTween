using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _mergeParticle;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public ParticleSystem MergeParticle => _mergeParticle;
}
