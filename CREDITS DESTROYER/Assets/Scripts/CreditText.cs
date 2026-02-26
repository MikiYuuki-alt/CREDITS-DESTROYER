using UnityEngine;

public class CreditText : MonoBehaviour
{
    [Header("落下スピード")]
    public float fallSpeed = 2.0f;

    void Update()
    {
        //落下
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 画面外に出たら消去
        if (transform.position.y < -6.0f)
        {
            Destroy(gameObject);
        }
    }
}