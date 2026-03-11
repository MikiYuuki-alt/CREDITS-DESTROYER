using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ★文字を操るための魔法の言葉
using UnityEngine.UI; // ★UIパネルを操るための魔法の言葉
using System.Collections; // ★コルーチン（時間待ち）を使うための魔法の言葉

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("プレイヤーの体力")]
    public int hp = 3;

    [Header("BGM用のオーディオソース")]
    public AudioSource bgmSource;

    [Header("リザルト演出用UI")]
    public TMP_Text resultText;          // 降ってくる文字
    public RectTransform resultTextRect; // 文字の位置（RectTransform）
    public Image fadePanel;              // フェードアウト用の黒パネル

    private bool isGameOver = false;
    private bool isClear = false;
    private PostProcessingEffect postEffect;

    void Awake()
    {
        instance = this;
        if (Camera.main != null)
        {
            postEffect = Camera.main.GetComponent<PostProcessingEffect>();
        }
    }

    void Start()
    {
        // 念のため、開始時に文字を上空(Y=800)にセット＆黒パネルを透明にしておく
        if (resultTextRect != null) resultTextRect.anchoredPosition = new Vector2(0, 800);
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0;
            fadePanel.color = c;
        }

        if (bgmSource != null && bgmSource.clip != null)
        {
            Invoke("GameClear", bgmSource.clip.length);
        }
    }

    public void TakeDamage()
    {
        if (isGameOver || isClear) return;

        hp--;
        Debug.Log("ダメージ！ 残りHP: " + hp);

        if (postEffect != null)
        {
            postEffect.PlaySpecialEffect(0.4f, 0.3f);
        }

        if (hp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        if (bgmSource != null) bgmSource.Stop();

        // ★ゲームオーバー演出の開始！
        StartCoroutine(ResultRoutine(false));
    }

    void GameClear()
    {
        if (isGameOver) return;
        isClear = true;

        // ★ゲームクリア演出の開始！
        StartCoroutine(ResultRoutine(true));
    }

    // ==========================================
    // ★追加：リザルト演出（文字落下 ＆ 暗転フェード）
    // ==========================================
    IEnumerator ResultRoutine(bool isClearMode)
    {
        // 1. 文字と色を決定する
        if (isClearMode)
        {
            resultText.text = "GAME CLEAR";
            resultText.color = Color.yellow; // クリアは金色（黄色）
        }
        else
        {
            resultText.text = "GAME OVER";
            resultText.color = Color.red;    // ゲームオーバーは赤色
        }

        // 2. 文字を上空から画面中央(Y=0)に「0.5秒」かけて落とす
        float dropDuration = 1.0f;
        float elapsed = 0f;
        Vector2 startPos = new Vector2(0, 800); // 始まり（上空）
        Vector2 endPos = new Vector2(0, 0);     // 終わり（中央）

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            // 割合（0.0 〜 1.0）を計算して、滑らかに移動させる
            float t = elapsed / dropDuration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f); // シュッと降りてきてブレーキがかかる計算式！

            resultTextRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null; // 1フレーム待つ
        }
        resultTextRect.anchoredPosition = endPos; // 最後にピッタリ中央に合わせる

        // 3. プレイヤーに余韻を味わわせる（2.5秒待つ）
        yield return new WaitForSeconds(2.5f);

        // 4. 黒い画面を「1秒」かけてフェードイン（真っ暗にする）
        float fadeDuration = 1.0f;
        elapsed = 0f;
        Color panelColor = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelColor.a = elapsed / fadeDuration; // 透明度を徐々に1.0にする
            fadePanel.color = panelColor;
            yield return null;
        }

        // 5. 完全に暗転したら、タイトルシーンへワープ！
        SceneManager.LoadScene("TitleScene");
    }
}