using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UvControll))]
public class UvAnimationControll : MonoBehaviour
{
    private UvControll uv;
    public UvControll UV
    {
        get { return uv == null ? (uv = GetComponent<UvControll>()) : uv; }
    }

    private Dictionary<string,SequenceData> _animationData = new Dictionary<string, SequenceData>();
    private Coroutine animationCoroutine;

    public void AddAnimatoin(string name, int start, int count, int time)
    {
        _animationData[name] = new SequenceData(){name = name,start = start-1,count = count,time = time};   //インデックスが1オリジンぽかったので、startは補正で-1してる
    }

    public void Play(string animation)
    {
        SequenceData data;
        if (_animationData.TryGetValue(animation, out data) == false) return;   //なかった
        if(animationCoroutine != null)StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimatoinIterator(data));
    }

    private IEnumerator AnimatoinIterator(SequenceData data)
    {
        while (true)
        {
            for (var index = data.start; index < data.start + data.count; index++)
            {
                UV.yIndex = index / UV.xMax;
                UV.xIndex = index % UV.xMax;
                yield return new WaitForSeconds(data.time / 1000.0f);
            }
        }
    }
}