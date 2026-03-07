using UnityEngine;

public class StaffRollWord : MonoBehaviour
{
    public float fallSpeed = 3.0f; // 落ちるスピード
    public bool isTrap = false;    // 「Thank you」の罠かどうか

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // ① Y座標が -7.0 より下になったか？
        if (transform.position.y < -7.0f)
        {
            Debug.Log("①：画面下(-7.0)のラインを通過しました！");

            // ② 罠（Trap）じゃない設定になっているか？
            if (!isTrap)
            {
                Debug.Log("②：罠ではないので、ダメージ命令を出します！");

                // ③ GameManagerはちゃんと存在しているか？
                if (GameManager.instance != null)
                {
                    Debug.Log("③：GameManagerを発見！TakeDamageを実行します！");
                    GameManager.instance.TakeDamage();
                }
                else
                {
                    Debug.LogError("🚨大問題：GameManagerが見つかりません！");
                }
            }
            else
            {
                Debug.Log("※これは罠(Thank you)なのでダメージはスルーします");
            }

            Destroy(gameObject);
        }
    }
}