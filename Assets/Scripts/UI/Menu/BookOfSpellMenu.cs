using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Menu
{
    public class BookOfSpellMenu : Menu
    {
        public override void Close()
        {
            IsOpen = false;
            GameManager._instance.enabled           = true;
            GameManager._instance.Player.enabled    = true;
            gameObject.SetActive(IsOpen);
        }

        public override void Open()
        {
            IsOpen = true;
            GameManager._instance.enabled           = false;
            GameManager._instance.Player.enabled    = false;
            gameObject.SetActive(IsOpen);
        }
    }
}
