using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public float pitchMin; // Pitch mínimo
    public float pitchMax; // Pitch máximo

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    public float GetRandomPitch()
    {
        return Random.Range(pitchMin, pitchMax);
    }
}