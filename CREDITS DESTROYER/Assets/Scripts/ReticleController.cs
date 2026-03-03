using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [Header("射撃時の効果音")]
    public AudioClip shootSound;

    void Start()
    {
        // ゲーム開始時に、Windows/Macの標準マウスカーソルを見えなくする
        Cursor.visible = false;
    }

    void Update()
    {
        // マウスの画面上の位置を、ゲーム内のワールド座標に変換してレティクルを移動
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;

        // 左クリックされた瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // 射撃音をカメラの位置で鳴らす（音が途切れないように）
            if (shootSound != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
            }

            // 射撃の当たり判定処理へ
            Shoot(worldPos);
        }
    }

    void Shoot(Vector2 targetPos)
    {
        // クリックした位置の「奥」に向かってRay（見えないレーザー）を飛ばす
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero);

        // 1. まず「何かに当たったか」と「それがEnemyタグを持っているか」を確認（Nullエラー防止！）
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // 2. 当たった相手が「特別な演出付きの文字（Thank you等）」か調べる
            SpecialTextShatter specialShatterScript = hit.collider.gameObject.GetComponent<SpecialTextShatter>();

            if (specialShatterScript != null)
            {
                // 画面揺れ＆赤フラッシュ付きの特別な破壊を実行して、ここで終了！
                specialShatterScript.Shatter();
                return;
            }

            // 3. 特別な文字じゃなかったら、いつもの「普通の文字」か調べる
            TextShatterEffect regularShatterScript = hit.collider.gameObject.GetComponent<TextShatterEffect>();

            if (regularShatterScript != null)
            {
                // いつものバラバラ破壊を実行して、ここで終了！
                regularShatterScript.Shatter();
                return;
            }

            // 4. 万が一、どちらのスクリプトも付いていないEnemyだった場合はただ消滅させる（エラー回避のお守り）
            Destroy(hit.collider.gameObject);
        }
    }
}