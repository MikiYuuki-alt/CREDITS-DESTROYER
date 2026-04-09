using UnityEngine;
using UnityEngine.Audio;
using System.Collections; // ★時間を待つ魔法を使うために追加

public class VolumeLoader : MonoBehaviour
{
    [Header("オーディオミキサーを入れてね")]
    public AudioMixer audioMixer;

    void Start()
    {
        // そのまま実行せず、「0.1秒待つ」という命令（コルーチン）を呼び出す
        StartCoroutine(SetVolumeWithDelay());
    }

    IEnumerator SetVolumeWithDelay()
    {
        // ★ミキサーが完全に起き上がるまで 0.1秒だけ待つ！
        yield return new WaitForSeconds(0.1f);

        // タイトル画面でセーブされた音量を読み込む
        float savedBGM = PlayerPrefs.GetFloat("SavedBGM", 1.0f);
        float savedSE = PlayerPrefs.GetFloat("SavedSE", 1.0f);

        // ミキサーに反映させる
        audioMixer.SetFloat("BGMVol", Mathf.Log10(savedBGM) * 20f);
        audioMixer.SetFloat("SEVol", Mathf.Log10(savedSE) * 20f);

        // （確認用）ログを出して、ちゃんとロードされたかチェック！
        Debug.Log("🔊 音量をロードしました！ BGM: " + savedBGM + " / SE: " + savedSE);
    }
}