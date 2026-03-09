using UnityEngine;
using UnityEngine.SceneManagement; // 画面切り替え用

public class GameManager : MonoBehaviour
{
    // ★魔法の変数：他のスクリプトから「GameManager.instance.〇〇」でアクセスできるようになる
    public static GameManager instance;

    [Header("プレイヤーの体力")]
    public int hp = 3;

    [Header("BGM用のオーディオソース")]
    public AudioSource bgmSource;

    private bool isGameOver = false;
    private bool isClear = false;

    void Awake()
    {
        // ゲーム開始時に「司令塔は私です」と登録する
        instance = this;
    }

    void Start()
    {
        // BGMの長さを測り、その秒数が経過したら「GameClear」という必殺技を自動で発動させる！
        if (bgmSource != null && bgmSource.clip != null)
        {
            Invoke("GameClear", bgmSource.clip.length);
        }
    }

    // 敵を逃した時や、罠を撃ってしまった時に呼ばれる
    public void TakeDamage()
    {
        if (isGameOver || isClear) return; // 終わってたら何もしない

        hp--;
        Debug.Log("ダメージ！ 残りHP: " + hp); // 画面左下に文字が出ます

        // 3回ミスしたらゲームオーバー
        if (hp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER...（3回落ちた）");

        if (bgmSource != null) bgmSource.Stop(); // BGMを止める

        // とりあえずタイトル画面に強制送還する（後でゲームオーバー画面を作ってもOK！）
        SceneManager.LoadScene("TitleScene");
    }

    void GameClear()
    {
        if (isGameOver) return; 

        isClear = true;
        Debug.Log("GAME CLEAR!!（生き残った！）");

        // とりあえずタイトル画面に戻す
        SceneManager.LoadScene("TitleScene");
    }
}