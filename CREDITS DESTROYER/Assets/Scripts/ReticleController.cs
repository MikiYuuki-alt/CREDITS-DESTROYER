using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [Header("射撃時の効果音")]
    public AudioClip shootSound;

    private AudioSource audioSource; // ★追加：SE用スピーカーを入れる箱

    void Start()
    {
        // ★追加：自分にくっついている AudioSource を取得
        audioSource = GetComponent<AudioSource>();

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
            // ★修正：PlayClipAtPointをやめて、自分のAudioSourceから鳴らす
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // 射撃の当たり判定処理へ
            Shoot(worldPos);
        }
    }

    void Shoot(Vector2 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            SpecialTextShatter specialShatterScript = hit.collider.gameObject.GetComponent<SpecialTextShatter>();
            if (specialShatterScript != null)
            {
                specialShatterScript.Shatter();
                return;
            }

            TextShatterEffect regularShatterScript = hit.collider.gameObject.GetComponent<TextShatterEffect>();
            if (regularShatterScript != null)
            {
                regularShatterScript.Shatter();
                return;
            }

            Destroy(hit.collider.gameObject);
        }
    }
}