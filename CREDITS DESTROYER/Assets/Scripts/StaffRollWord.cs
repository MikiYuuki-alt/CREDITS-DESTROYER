using UnityEngine;

public class StaffRollWord : MonoBehaviour
{
    public float fallSpeed = 3.0f; // 落ちるスピード
    public bool isTrap = false;    // 「Thank you」の罠かどうか

    void Update()
    {
        // スタッフロールのように、等速で下へスーーッと落ちる
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 画面の下（Y座標が-7.0より下）に落ち切ったら
        if (transform.position.y < -7.0f)
        {
            // 罠（Thank you）じゃない普通の敵なら、見逃したのでダメージ！
            if (!isTrap)
            {
                GameManager.instance.TakeDamage();
            }

            // 画面外に消えたので自分を消去
            Destroy(gameObject);
        }
    }
}