using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextShatterEffect : MonoBehaviour
{
    [Header("破壊時の効果音")]
    public AudioClip shatterSound;

    private TMP_Text originalTextComponent;

    void Awake()
    {
        
        originalTextComponent = GetComponent<TextMeshPro>();
    }

    public void Shatter()
    {
        // 1. 効果音を鳴らす（カメラの位置で鳴らして途切れを防ぐ）
        if (shatterSound != null)
        {
            AudioSource.PlayClipAtPoint(shatterSound, Camera.main.transform.position);
        }

        // 2. 文字情報を最新にする（座標ズレを防ぐための必須処理）
        originalTextComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = originalTextComponent.textInfo;

        // 3. 1文字ずつループして破片を作る
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // 空白などの見えない文字はスキップ
            if (!charInfo.isVisible) continue;

            // --- 破片オブジェクトの生成と配置 ---
            GameObject piece = new GameObject($"Piece_{charInfo.character}");

            // 文字の中心座標を計算してワールド座標に変換
            Vector3 centerPos = (charInfo.bottomLeft + charInfo.topRight) / 2.0f;
            Vector3 worldPos = transform.TransformPoint(centerPos);

            piece.transform.position = worldPos;
            piece.transform.rotation = transform.rotation;
            piece.transform.localScale = transform.localScale;

            // --- 見た目（TextMeshPro）のコピー ---
            TextMeshPro pieceTMP = piece.AddComponent<TextMeshPro>();
            pieceTMP.font = originalTextComponent.font;
            pieceTMP.fontSharedMaterial = originalTextComponent.fontSharedMaterial; // 豆腐化防止！
            pieceTMP.text = charInfo.character.ToString();
            pieceTMP.fontSize = originalTextComponent.fontSize;
            pieceTMP.color = originalTextComponent.color;
            pieceTMP.alignment = TextAlignmentOptions.Center;

            // --- 物理演算（ぶっ飛ばす処理） ---
            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0.5f; // 重力を少し弱くしてフワッと落下させる

            // 少しだけランダムな方向に弾けさせる
            Vector2 randomDir = new Vector2(Random.Range(-2f, 2f), Random.Range(0.5f, 2f));
            rb.AddForce(randomDir, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);

            // --- 破片のお掃除 ---
            // 作った破片に「フェードアウト係」をくっつけて、綺麗に消滅させる
            piece.AddComponent<PieceFadeOut>();
        }

        // 4. 最後に、元の大きなテキスト本体を消去する
        Destroy(gameObject);
    }
}