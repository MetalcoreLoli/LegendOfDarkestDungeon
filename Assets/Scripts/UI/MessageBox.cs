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
        OK = 0,
        Cancel  = 1
    }
    public class MessageBox : MonoBehaviour
    {
        [SerializeField] Text Tile;
        [SerializeField] Text Message;
        public DialogResult DialogResult { get; private set; }

        public DialogResult Show(string title, string message) 
        {
            gameObject.SetActive(true);
            Tile.text = title;
            Message.text = message;

            return DialogResult;
        }

        public void OkClick()
        {
            DialogResult = DialogResult.OK;
            gameObject.SetActive(false);
        }

        public void CancelClick()
        { 
            DialogResult = DialogResult.Cancel;
            gameObject.SetActive(false);
        }
    }
}
