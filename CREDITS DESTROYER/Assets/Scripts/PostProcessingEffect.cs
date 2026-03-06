using UnityEngine;
using UnityEngine.UI; // UIを操作するのに必要
using System.Collections; // コルーチンを使うのに必要

public class PostProcessingEffect : MonoBehaviour
{
    [Header("UIパネル（赤い画面）")]
    public Image flashPanel;

    private Vector3 originalPos;

    void Start()
    {
        // カメラの元の位置を記憶しておく
        originalPos = transform.localPosition;
    }

    // 外部（敵のスクリプト）から呼び出される必殺技
    public void PlaySpecialEffect(float intensity, float duration)
    {
        // すでに揺れていたらストップさせる
        StopAllCoroutines();
        // フラッシュとシェイクの「コルーチン」を同時に開始！
        StartCoroutine(FlashCoroutine(intensity * 0.5f, duration));
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    // --- 画面フラッシュの処理（コルーチン） ---
    IEnumerator FlashCoroutine(float maxAlpha, float duration)
    {
        if (flashPanel == null) yield break;

        // 1. パネルを一瞬で赤く（Alphaを上げる）する
        Color color = flashPanel.color;
        color.a = maxAlpha;
        flashPanel.color = color;

        // 2. 指定した時間（duration）かけて、徐々に透明に戻す
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // イージング（滑らかに）させて透明度を下げる
            color.a = Mathf.Lerp(maxAlpha, 0f, elapsed / duration);
            flashPanel.color = color;
            yield return null; // 1フレーム待つ
        }

        // 3. 最後に完全に透明にする
        color.a = 0f;
        flashPanel.color = color;
    }

    // --- 画面シェイクの処理（コルーチン） ---
    IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;

        // 指定した時間（duration）の間、カメラを揺らし続ける
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // ランダムな位置オフセットを計算（揺れ）
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            // カメラの位置にオフセットを適用
            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            yield return null; 
        }

        // 最後にカメラの位置を元に戻す
        transform.localPosition = originalPos;
    }
}