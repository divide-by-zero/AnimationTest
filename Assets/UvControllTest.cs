using UnityEngine;
using UnityEngine.UI;

public class UvControllTest : MonoBehaviour
{
    [SerializeField]
    private UvAnimationControll _target;

    [SerializeField]
    private Text _debugText;

	void Start () {
        _target.UV.uvRect = new Rect(0,0,128,128);  //１アニメーションのサイズを指定
	    _target.AddAnimatoin("waiting", 17, 1, 500);//アニメーション名とスタートindex、枚数、パラパラアニメ秒数
	    _target.AddAnimatoin("walking", 9, 8, 100);
	    _target.AddAnimatoin("jump", 2, 2, 500);
	    _target.AddAnimatoin("down", 4, 1, 500);

        _target.Play("walking");
	}

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            var animTarget = new[]{
                "waiting",
                "walking",
                "jump",
                "down"
            };

            var randomAnimName = animTarget[Random.Range(0, 4)];
            _debugText.text = "animation=" + randomAnimName;
            _target.Play(randomAnimName);
        }
	}
}
