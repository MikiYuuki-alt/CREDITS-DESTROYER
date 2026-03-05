using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // ★変更：Update関数を消して、ボタンから呼び出せる専用の命令（public void）を作りました！
    // ※「public」をつけることで、Unityの画面上からこの命令をセットできるようになります。
    public void OnClickStartButton()
    {
        // メインゲームのシーンへ移動する
        // ※シーン名が違う場合は "SampleScene" を書き換えてください
        SceneManager.LoadScene("GameScene");
    }
}