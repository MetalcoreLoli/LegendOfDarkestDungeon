﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Factory
{
    public class DungeonFactoryManager : MonoBehaviour
    {
        DefaultDungeonFactory dungeonFactory;
        BlueDungeonFactory dungeonBlueFactory;

        public static DungeonFactoryManager instance;
        public DefaultDungeonFactory DefaultDungeonFactory { get => dungeonFactory; private set => dungeonFactory = value; }
        public BlueDungeonFactory BlueDungeonFactory { get => dungeonBlueFactory; private set => dungeonBlueFactory = value; }

        //public static DungeonFactoryManager Get()
        //{
        //    return GameObject.Find("DungeonManager").GetComponent<DungeonFactoryManager>();
        //}

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            dungeonFactory = GetComponent<DefaultDungeonFactory>();
            dungeonFactory.Size = new Vector2(70, 40);
            dungeonBlueFactory = GetComponent<BlueDungeonFactory>();
            dungeonBlueFactory.Size = new Vector2(70, 40);
        }
    }
}
