using UnityEngine;
using UnityEngine.UI;

public class KDH_VolumeSettings : MonoBehaviour
{
    [Header("볼륨 슬라이더 연결")]
    public Slider volumeSlider;

    private void Start()
    {
        //만약 슬라이더가 연결되어 있으면
        if (volumeSlider != null)
        {
            //게임 시작 시, 슬라이더의 위치를 현재 게임의 볼륨과 똑같이 맞춤.
            volumeSlider.value = AudioListener.volume;

            //슬라이더의 값이 변할 때마다 'ChangeVolume' 함수가 실행됨
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    public void ChangeVolume(float value)
    {
        //AudioListener,volume은 게임 내의 모든 소리를 총괄함
        AudioListener.volume = value;
    }
}