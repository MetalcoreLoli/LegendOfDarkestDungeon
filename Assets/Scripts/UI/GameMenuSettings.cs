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
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.menuMusicSource.Play();
        }
        public void Close()
        {
            gameObject.SetActive(false);
            IsOpen = false;
            SoundManager.instance.musicSource.Play();
            SoundManager.instance.menuMusicSource.Stop();
        }

        public void Save()
        {
            GameManager._instance.dataManager.SaveData();
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


  
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            //Application.Quit();
        }
    }
}
