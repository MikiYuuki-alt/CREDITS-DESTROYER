using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public GameObject explosionPrefab;

    void Start()
    {
        // OS標準の矢印カーソルを消す
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. レティクルをマウスの位置に移動させる
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;

        // 2. 左クリックで射撃
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(worldPos);
        }
    }

    void Shoot(Vector2 targetPos)
    {
        // クリックした場所にある当たり判定（コライダー）を探す
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero);

        // もし何かに当たっていて、それが「Enemy」というタグだったら
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // エフェクトがあれば生成して、敵を破壊する
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, hit.collider.transform.position, Quaternion.identity);
            }
            // 当たった相手(hit.collider.gameObject)にくっついている
            // "TextShatterEffect" というスクリプトを探す
            TextShatterEffect shatterScript = hit.collider.gameObject.GetComponent<TextShatterEffect>();

            // もしそのスクリプトが見つかったら
            if (shatterScript != null)
            {
                // バラバラにする命令を出す！
                shatterScript.Shatter();
            }
            else
            {
                // もしバラバラ化スクリプトが付いてない普通の敵なら、今まで通りただ消す
                Destroy(hit.collider.gameObject);
            }
            Destroy(hit.collider.gameObject);
        }
    }
}