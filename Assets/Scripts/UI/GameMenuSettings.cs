﻿using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class GameMenuSettings : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public void Open()
        {
            if (GameManager._instance.inventoryManager.IsOpen)
                GameManager._instance.inventoryManager.IsOpen = !GameManager._instance.inventoryManager.IsOpen;

            gameObject.SetActive(true);
            IsOpen = true;
            GameManager._instance.enabled = false;
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.menuMusicSource.Play();
        }
        public void Close()
        {
            gameObject.SetActive(false);
            IsOpen = false;
            SoundManager.instance.musicSource.Play();
            SoundManager.instance.menuMusicSource.Stop();
            GameManager._instance.enabled = true;
        }

        public void Save()
        {
            GameManager._instance.dataManager.SaveData();
        }

        public void OpenInventory()
        {
            GameManager._instance.inventoryManager.IsOpen = !GameManager._instance.inventoryManager.IsOpen;
            Close();
        }
        public void OpenOptions()
        {
           
            var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
            ui.optionMenu.Open();
            Close();
        }
        
        public void Exit()
        {
            Destroy(GameObject.Find("SoundManager"));
            Destroy(GameObject.Find("GameManager(Clone)")); 
            var board = GameObject.Find("Board");
            GameObject.Destroy(board);
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            foreach (var enem in enemies)
                Destroy(enem);
            SaveLoader.Instance().IsNeedToLoad = false;



            SceneManager.LoadScene(0, LoadSceneMode.Single);
            //Application.Quit();
        }
    }
}
