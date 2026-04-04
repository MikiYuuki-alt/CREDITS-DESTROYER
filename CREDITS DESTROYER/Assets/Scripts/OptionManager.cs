using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    [Header("UIパーツ")]
    public GameObject optionPanel;
    public Slider bgmSlider;
    public Slider seSlider;

    [Header("オーディオミキサー")]
    public AudioMixer audioMixer;

    void Start()
    {
        optionPanel.SetActive(false);

        // ★追加：セーブデータから音量を読み込む！（データが無ければ 1.0 にする）
        float savedBGM = PlayerPrefs.GetFloat("SavedBGM", 1.0f);
        float savedSE = PlayerPrefs.GetFloat("SavedSE", 1.0f);

        bgmSlider.minValue = 0.0001f;
        bgmSlider.maxValue = 1.0f;
        bgmSlider.value = savedBGM; // スライダーの位置をセーブデータに合わせる

        seSlider.minValue = 0.0001f;
        seSlider.maxValue = 1.0f;
        seSlider.value = savedSE;

        // 読み込んだ音量を実際のミキサーに反映させる
        SetBGM(savedBGM);
        SetSE(savedSE);

        bgmSlider.onValueChanged.AddListener(SetBGM);
        seSlider.onValueChanged.AddListener(SetSE);
    }

    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
        PlayerPrefs.Save(); // ★追加：閉じる時に確実にセーブデータを保存する！
    }

    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SavedBGM", volume); // ★追加：動かすたびにBGM音量をセーブ
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SavedSE", volume); // ★追加：動かすたびにSE音量をセーブ
    }
}