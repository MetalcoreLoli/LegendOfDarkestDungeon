using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] protected bool _isOpen;

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                gameObject.SetActive(_isOpen);
            }
        }

        public abstract void Open();

        public abstract void Close();
    }
}