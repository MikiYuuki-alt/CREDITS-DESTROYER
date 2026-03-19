using UnityEngine;
using TMPro; // 文字(TextMeshPro)を書き換えるために必要！

public class StaffTextSpawner : MonoBehaviour
{
    [Header("生み出す敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("出現させる文字のリスト（自由に追加・変更できます）")]
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
            // 出現位置をランダムに決める
            float randomX = Random.Range(-6.0f, 6.0f);
            Vector3 spawnPos = new Vector3(randomX, 7.0f, 0f);

            // 1. まずはベースとなるプレハブ（器）を出現させる
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // 2. リストの中からランダムに文字を1つ選ぶ
            if (staffTitles.Length > 0)
            {
                int randomIndex = Random.Range(0, staffTitles.Length);
                string selectedTitle = staffTitles[randomIndex];

                // 3. 出現させた敵の「TextMeshPro」を見つけて、選んだ文字に書き換える！
                TMP_Text tmp = spawnedEnemy.GetComponent<TMP_Text>();
                if (tmp != null)
                {
                    tmp.text = selectedTitle;
                }
            }
        }
    }
}