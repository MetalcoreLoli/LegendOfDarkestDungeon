﻿using Assets.Scripts.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class OptionMenu : Menu
    {
        public InputField[] InputFields;

        private KeyCode pressedKey;
        private Array allKeyCodes;

        public override void Open()
        {
            gameObject.SetActive(true);
            GameManager.Instance.Player.enabled = false;
            GameManager.Instance.enabled = false;
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            GameManager.Instance.Player.enabled = true;
            GameManager.Instance.enabled = true;
        }

        public void Done()
        {
            GameManager.MessageBox.Show("Saving", "Do you want to save changes?");
            if (GameManager.MessageBox.DialogResult == DialogResult.OK)
            {
                GameManager.MessageBox.ResetResult();
                Close();
            }
        }

        private void Awake()
        {
            allKeyCodes = System.Enum.GetValues(typeof(KeyCode));
        }

        private void Update()
        {
        }

        private void OnGUI()
        {
            //Event e = Event.current;
            //if (e.isKey)
            //{
            //    pressedKey = e.keyCode;
            //    Debug.Log(pressedKey);
            //}
        }

        public void OnEdit(InputField inputField)
        {
            foreach (KeyCode key in allKeyCodes)
            {
                if (Input.GetKeyDown(key))
                    pressedKey = key;
            }
            inputField.text = pressedKey.ToString();
            GameInput.SetKey(inputField.GetComponent<InputFiledInfo>().Name, pressedKey);
        }
    }
}