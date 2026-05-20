using UnityEngine;

public class LSO_SoundManager : MonoBehaviour
{
    public static LSO_SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void SfxPlay(string sfxName,AudioClip clip)
    {
        GameObject go = new GameObject(sfxName+"Sound");
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(go, clip.length);
    }
}
