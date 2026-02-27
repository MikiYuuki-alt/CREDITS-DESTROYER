using UnityEngine;
using TMPro; // TextMeshProを操作するのに必要

public class PieceFadeOut : MonoBehaviour
{
    public float delay = 0.5f;        // 消え始めるまでの待機時間（秒）
    public float fadeDuration = 0.5f; // フェードアウトにかかる時間（秒）

    private TextMeshPro tmp;
    private float timer = 0f;
    private Color startColor;

    void Start()
    {
        // 自分のTextMeshProを取得して、最初の色を記憶しておく
        tmp = GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            startColor = tmp.color;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 待機時間を過ぎたらフェード開始！
        if (timer > delay && tmp != null)
        {
            // フェードの進行度合い（0.0 ～ 1.0）を計算
            float progress = (timer - delay) / fadeDuration;

            // 色の透明度（Alpha値）を徐々に下げる
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startColor.a, 0f, progress);
            tmp.color = newColor;

            // 完全に透明（progressが1.0以上）になったら自分を消去
            if (progress >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}