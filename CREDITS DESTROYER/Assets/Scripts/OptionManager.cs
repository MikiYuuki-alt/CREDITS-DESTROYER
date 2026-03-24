using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // ★AudioMixerを使うための魔法の言葉

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
        // 最初にオプション画面は閉じておく
        optionPanel.SetActive(false);

        // スライダーの初期設定（音量が0にならないように 0.0001 ～ 1.0 にする）
        bgmSlider.minValue = 0.0001f;
        bgmSlider.maxValue = 1.0f;
        bgmSlider.value = 1.0f; // 最初はMAX

        seSlider.minValue = 0.0001f;
        seSlider.maxValue = 1.0f;
        seSlider.value = 1.0f; // 最初はMAX

        // スライダーを動かした時に、下の「SetBGM」「SetSE」を実行するように設定
        bgmSlider.onValueChanged.AddListener(SetBGM);
        seSlider.onValueChanged.AddListener(SetSE);
    }

    // オプションを開く
    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }

    // オプションを閉じる
    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }

    // BGMの音量を変える（※人間の耳に自然に聞こえるよう、対数(Log10)で計算します！）
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", Mathf.Log10(volume) * 20f);
    }

    // SEの音量を変える
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", Mathf.Log10(volume) * 20f);
    }
}