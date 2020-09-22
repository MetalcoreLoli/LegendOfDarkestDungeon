using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{

    public enum DialogResult 
    {
        None      = 0,
        OK        = 1,
        Cancel    = 2
    }
    public class MessageBox : MonoBehaviour
    {
        [SerializeField] private Text Tile;
        [SerializeField] private Text Message;
        [SerializeField] private DialogResult dialogResult = DialogResult.None;
        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;
        public DialogResult DialogResult { get => dialogResult; private set => dialogResult = value; } 

        public DialogResult Show(string title, string message) 
        {
            ResetResult();
            gameObject.SetActive(true);
            Tile.text = title;
            Message.text = message;
            okButton.onClick.AddListener(OkClick);
            cancelButton.onClick.AddListener(CancelClick);
            return DialogResult;
        }

        public void ResetResult()
        {
            okButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            DialogResult = DialogResult.None;
        }

        private void OkClick()
        {
            DialogResult = DialogResult.OK;
            Debug.Log("OK");
            gameObject.SetActive(false);
        }

        private void CancelClick()
        { 
            DialogResult = DialogResult.Cancel;
            Debug.Log("Cancel");
            gameObject.SetActive(false);
        }
    }
}
