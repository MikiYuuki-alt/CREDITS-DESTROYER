using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    [Header("生み出す敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("何秒ごとに生み出すか")]
    public float spawnInterval = 2.0f;

    private float timer = 0f;

    void Update()
    {
        // 毎フレーム、時間をカウントアップする
        timer += Time.deltaTime;

        // 設定した時間（spawnInterval）を超えたら
        if (timer >= spawnInterval)
        {
            SpawnEnemy(); // 敵を生み出す命令を実行！
            timer = 0f;   // タイマーを0に戻して再スタート
        }
    }

    void SpawnEnemy()
    {
        // もしプレハブがちゃんとセットされていたら
        if (enemyPrefab != null)
        {
            // 出現するX座標（横の位置）を -6.0 ～ 6.0 の間でランダムに決める
            float randomX = Random.Range(-6.0f, 6.0f);

            // 出現する場所を決定（Y=7.0で画面の上の方、Z=0）
            Vector3 spawnPos = new Vector3(randomX, 7.0f, 0f);

            // プレハブをもとに、指定した場所に敵を新しく作り出す！
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}