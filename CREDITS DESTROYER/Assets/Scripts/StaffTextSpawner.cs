using UnityEngine;
using TMPro;

public class StaffTextSpawner : MonoBehaviour
{
    [Header("生み出す敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("出現させる文字のリスト")]
    public string[] staffTitles = { "DIRECTOR", "PROGRAMMER", "DESIGNER", "SOUND", "QA" };

    [Header("何秒ごとに生み出すか")]
    public float spawnInterval = 2.0f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            float randomX = Random.Range(-6.0f, 6.0f);
            Vector3 spawnPos = new Vector3(randomX, 7.0f, 0f);

            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            if (staffTitles.Length > 0)
            {
                int randomIndex = Random.Range(0, staffTitles.Length);
                string selectedTitle = staffTitles[randomIndex];

                TMP_Text tmp = spawnedEnemy.GetComponent<TMP_Text>();
                if (tmp != null)
                {
                    // 1. まず文字をセットする
                    tmp.text = selectedTitle;

                    // ----------------------------------------------------
                    // ★ここから追加：当たり判定を文字にピッタリ合わせる魔法
                    // ----------------------------------------------------
                    BoxCollider2D col = spawnedEnemy.GetComponent<BoxCollider2D>();
                    if (col != null)
                    {
                        // TextMeshProに「今の文字でのサイズを大至急計算して！」と命令
                        tmp.ForceMeshUpdate();

                        // 計算された文字のピッタリの横幅・縦幅を取得
                        Vector2 textSize = tmp.textBounds.size;

                        // 当たり判定(BoxCollider2D)のサイズを、文字のサイズに上書きする！
                        // （※もし当たり判定がシビアすぎると感じたら、textSize.x + 0.5f のように少し足すと撃ちやすくなります）
                        col.size = new Vector2(textSize.x, textSize.y);

                        // 念のため、当たり判定の中心ズレをリセットしておく
                        col.offset = Vector2.zero;
                    }
                    // ----------------------------------------------------
                }
            }
        }
    }
}