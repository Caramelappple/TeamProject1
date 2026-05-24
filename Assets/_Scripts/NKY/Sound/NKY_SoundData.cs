using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NKY Sound Data", menuName = "SO/NKY Sound Data")]
public class NKY_SoundData : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Range(0f, 1f)] public float volume = 1f;

    [Header("Pitch Randomness (for SFX)")]
    [Range(0.5f, 2f)] public float minPitch = 0.95f;
    [Range(0.5f, 2f)] public float maxPitch = 1.05f;
}
