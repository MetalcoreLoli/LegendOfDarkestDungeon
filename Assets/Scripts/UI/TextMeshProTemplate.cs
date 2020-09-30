using UnityEngine;

namespace Assets.Scripts.UI
{
    [CreateAssetMenu(fileName = "TextMeshProTemplate", menuName = "new text", order = 52)]
    public class TextMeshProTemplate : ScriptableObject
    {
        [SerializeField] private GameObject text;
        public GameObject Text { get => text; set => text = value; }
    }
}