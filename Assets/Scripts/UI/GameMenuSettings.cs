using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class GameMenuSettings : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Save()
        {
            GameManager._instance.dataManager.SaveData();
        }

        public void Exit()
        {
            Destroy(GameObject.Find("GameManager(Clone)"));
            Destroy(GameObject.Find("SoundManager"));
            SceneManager.LoadScene(0, LoadSceneMode.Single);
           // Application.Quit();
        }
    }
}
