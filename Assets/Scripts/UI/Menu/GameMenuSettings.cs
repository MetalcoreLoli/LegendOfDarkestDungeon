using Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Menu
{
    public class GameMenuSettings : Menu
    {
        public override void Open()
        {
            if (GameManager.Instance.inventoryManager.IsOpen)
                GameManager.Instance.inventoryManager.IsOpen = !GameManager.Instance.inventoryManager.IsOpen;

            gameObject.SetActive(true);
            IsOpen = true;
            GameManager.Instance.enabled = false;
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.menuMusicSource.Play();
        }

        public override void Close()
        {
            gameObject.SetActive(false);
            IsOpen = false;
            SoundManager.instance.musicSource.Play();
            SoundManager.instance.menuMusicSource.Stop();
            GameManager.Instance.enabled = true;
        }

        public void Save()
        {
            GameManager.Instance.dataManager.SaveData();
        }

        public void OpenInventory()
        {
            GameManager.Instance.inventoryManager.IsOpen = !GameManager.Instance.inventoryManager.IsOpen;
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
            DestroyImmediate(GameObject.Find("SoundManager"));
            DestroyImmediate(GameObject.Find("GameManager(Clone)"));
            var board = GameObject.Find("Board");
            GameObject.DestroyImmediate(board);
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (var enem in enemies)
                DestroyImmediate(enem);
            SaveLoader.Instance().IsNeedToLoad = false;

            SceneManager.LoadScene(0, LoadSceneMode.Single);
            //Application.Quit();
        }
    }
}