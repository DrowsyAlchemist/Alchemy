using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 51)]
public class Settings : ScriptableObject
{
    [SerializeField] private ClosedElement _closedElementInfo;
    [SerializeField] private VideoAd _videoAdInfo;

    public ClosedElement ClosedElementInfo => _closedElementInfo;
    public VideoAd VideoAdInfo => _videoAdInfo;

    [Serializable]
    public class ClosedElement
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _lable;
        [SerializeField] private string _lableEn;

        public Sprite Sprite => _sprite;
        public string Lable => _lable;
    }

    [Serializable]
    public class VideoAd
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _lable;
        [SerializeField] private string _lableEn;

        public Sprite Sprite => _sprite;
        public string Lable => _lable;
    }
}
