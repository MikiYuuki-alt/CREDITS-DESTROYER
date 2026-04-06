using UnityEngine;
using UnityEngine.Audio;

public class VolumeLoader : MonoBehaviour
{
    [Header("オーディオミキサーを入れてね")]
    public AudioMixer audioMixer;

    void Start()
    {
        // タイトル画面でセーブされた音量を読み込む
        float savedBGM = PlayerPrefs.GetFloat("SavedBGM", 1.0f);
        float savedSE = PlayerPrefs.GetFloat("SavedSE", 1.0f);

        // ミキサーに反映させる
        audioMixer.SetFloat("BGMVol", Mathf.Log10(savedBGM) * 20f);
        audioMixer.SetFloat("SEVol", Mathf.Log10(savedSE) * 20f);
    }
}