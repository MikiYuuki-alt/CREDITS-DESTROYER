using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class SpecialTextShatter : MonoBehaviour
{
    public AudioClip shatterSound;

    // ★追加：演出担当のカメラ
    private PostProcessingEffect postEffect;

    private TMP_Text originalTextComponent;

    void Awake()
    {
        originalTextComponent = GetComponent<TextMeshPro>();
        // カメラにくっついている演出用スクリプトを探して記憶
        if (Camera.main != null)
        {
            postEffect = Camera.main.GetComponent<PostProcessingEffect>();
        }
    }

    public void Shatter()
    {
        GameManager.instance.TakeDamage();
        // 砕ける瞬間に、強烈なフラッシュとシェイクを実行！（強さ0.3f、時間0.4秒）
        if (postEffect != null)
        {
            postEffect.PlaySpecialEffect(0.3f, 0.4f);
        }

        if (shatterSound != null)
        {
            AudioSource.PlayClipAtPoint(shatterSound, Camera.main.transform.position);
        }

        originalTextComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = originalTextComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            GameObject piece = new GameObject($"Piece_{charInfo.character}");
            Vector3 centerPos = (charInfo.bottomLeft + charInfo.topRight) / 2.0f;
            Vector3 worldPos = transform.TransformPoint(centerPos);
            piece.transform.position = worldPos;
            piece.transform.rotation = transform.rotation;
            piece.transform.localScale = transform.localScale;

            TextMeshPro pieceTMP = piece.AddComponent<TextMeshPro>();
            pieceTMP.font = originalTextComponent.font;
            pieceTMP.fontSharedMaterial = originalTextComponent.fontSharedMaterial;
            pieceTMP.text = charInfo.character.ToString();
            pieceTMP.fontSize = originalTextComponent.fontSize;
            pieceTMP.color = originalTextComponent.color;
            pieceTMP.alignment = TextAlignmentOptions.Center;

            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            // ★特別な敵なので、破片を少し派手に弾けさせる
            Vector2 randomDir = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 5f));
            rb.AddForce(randomDir, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-10f, 10f), ForceMode2D.Impulse);
            rb.gravityScale = 0.3f; // フワッと落下

            // 以前作ったフェードアウトを追加
            piece.AddComponent<PieceFadeOut>();
        }

        Destroy(gameObject);
    }
}