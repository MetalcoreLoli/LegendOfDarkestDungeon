﻿using System;
using System.Linq;
using Random = System.Random;

namespace Assets.Scripts.Dices
{
    public class Dice
    {
        private int rolledValue = 0;

        private Edge rolledEdge;
        public UInt16 CountOfEdges { get; set; }

        public Int32 RolledValue
        {
            get
            {
                if (rolledValue == 0)
                    Roll();
                return rolledValue;
            }
        }

        public Edge[] Edges { get; set; }

        public Dice(UInt16 countOfEdge)
        {
            CountOfEdges = countOfEdge;
            Edges = CreateDice(CountOfEdges);
        }

        private Edge[] CreateDice(UInt16 countOfEdges)
        {
            Edge[] temp = new Edge[countOfEdges];
            for (int i = 0; i < countOfEdges; i++)
                temp[i] = new Edge(i + 1);
            return temp;
        }

        public int Roll()
        {
            int randIndex = new Random(DateTime.Now.Millisecond).Next(0, CountOfEdges);
            rolledValue = Edges[randIndex].Value;
            rolledEdge = Edges[randIndex];
            return rolledValue;
        }

        public int Roll(Random rand)
        {
            int randIndex = rand.Next(0, CountOfEdges);
            rolledValue = Edges[randIndex].Value;
            rolledEdge = Edges[randIndex];
            return rolledValue;
        }

        public static Dice Parser(string dice)
        {
            ushort countOfEdges = ushort.Parse(dice.Split('d').Last());
            return new Dice(countOfEdges);
        }

        public static implicit operator Dice(string dice)
            => Parser(dice);
    }
}