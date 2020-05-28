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
        [SerializeField] Text Tile;
        [SerializeField] Text Message;
        [SerializeField] DialogResult dialogResult = DialogResult.None;
        public DialogResult DialogResult { get => dialogResult; private set => dialogResult = value; } 

        public DialogResult Show(string title, string message) 
        {
            ResetResult();
            gameObject.SetActive(true);
            Tile.text = title;
            Message.text = message;

            return DialogResult;
        }

        public void ResetResult()
        {
            DialogResult = DialogResult.None;
        }

        public void OkClick()
        {
            DialogResult = DialogResult.OK;
            Debug.Log("OK");
            gameObject.SetActive(false);
        }


        public void CancelClick()
        { 
            DialogResult = DialogResult.Cancel;
            Debug.Log("Cancel");
            gameObject.SetActive(false);
        }
    }
}
