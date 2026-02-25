using UnityEngine;
using TMPro; // TextMeshProを使うために必要

[RequireComponent(typeof(TextMeshPro))]
public class TextShatterEffect : MonoBehaviour
{
    private TMP_Text originalTextComponent;
    public AudioClip shatterSound;
    void Awake()
    {
        originalTextComponent = GetComponent<TMP_Text>();
    }

    // この関数が外部から呼ばれると、バラバラになる
    public void Shatter()
    {
        // ★追加：砕ける瞬間に、カメラの位置で破壊音を鳴らす
        if (shatterSound != null)
        {
            AudioSource.PlayClipAtPoint(shatterSound, Camera.main.transform.position);
        }
        // 最新の文字情報を強制的に更新
        originalTextComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = originalTextComponent.textInfo;

        // 1文字ずつループ処理
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // 空白文字や見えない文字はスキップ
            if (!charInfo.isVisible) continue;

            // --- 1文字分の破片オブジェクトを作成 ---
            GameObject piece = new GameObject($"Piece_{charInfo.character}");

            // 元のテキストと同じ位置・回転・大きさに合わせる
            // (文字ごとの中心位置をワールド座標に変換して配置)
            Vector3 centerPos = (charInfo.bottomLeft + charInfo.topRight) / 2.0f;
            Vector3 worldPos = transform.TransformPoint(centerPos);
            piece.transform.position = worldPos;
            piece.transform.rotation = transform.rotation;
            piece.transform.localScale = transform.localScale;

            // --- TextMeshProコンポーネントを追加して設定をコピー ---
            TextMeshPro pieceTMP = piece.AddComponent<TextMeshPro>();
            // 元のフォントアセットとマテリアルをコピー（重要！）
            pieceTMP.font = originalTextComponent.font;
            pieceTMP.fontSharedMaterial = originalTextComponent.fontSharedMaterial;
            // その1文字だけを表示するように設定
            pieceTMP.text = charInfo.character.ToString();
            pieceTMP.fontSize = originalTextComponent.fontSize;
            pieceTMP.color = originalTextComponent.color;
            pieceTMP.alignment = TextAlignmentOptions.Center;

            // --- 物理演算を追加して落下させる ---
            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            // 少しだけランダムな方向に弾けさせる（お好みで調整）
            Vector2 randomDir = new Vector2(Random.Range(-2f, 2f), Random.Range(0.5f, 2f));
            rb.AddForce(randomDir, ForceMode2D.Impulse);
            // 回転力も少し加える
            rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);

            // --- 破片のお掃除 ---
            // 3秒後に破片を自動で消す（メモリ節約）
            Destroy(piece, 3.0f);
        }

        // 最後に、元の大きなテキスト自体を消す
        Destroy(gameObject);
    }
}