using System;
using System.Collections.Generic;

namespace lab5v9
{
    public class Tool
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Tool(string name)
        {
            this.name = name;
        }

        public virtual void Use()
        {
            Console.WriteLine($"Використовується інструмент {Name}.");
        }
    }

    public class Hammer : Tool
    {
        private double weight;

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public Hammer(string name, double weight) : base(name)
        {
            this.weight = weight;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} ({Weight} кг) забиває цвях: бах-бах!");
        }
    }

    public class Screwdriver : Tool
    {
        private string tipType;

        public string TipType
        {
            get { return tipType; }
            set { tipType = value; }
        }

        public Screwdriver(string name, string tipType) : base(name)
        {
            this.tipType = tipType;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} (наконечник {TipType}) закручує гвинт.");
        }
    }

    public class Wrench : Tool
    {
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public Wrench(string name, int size) : base(name)
        {
            this.size = size;
        }

        public override void Use()
        {
            Console.WriteLine($"{Name} на {Size} мм затягує гайку.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Tool> tools = new List<Tool>();
            tools.Add(new Hammer("Молоток", 0.5));
            tools.Add(new Screwdriver("Викрутка", "хрестовий"));
            tools.Add(new Wrench("Гайковий ключ", 13));
            tools.Add(new Hammer("Кувалда", 3));
            tools.Add(new Screwdriver("Мала викрутка", "плаский"));

            Console.WriteLine("=== Поліморфний виклик Use() ===");
            List<string> used = new List<string>();
            foreach (Tool t in tools)
            {
                t.Use();
                used.Add($"{t.Name} ({t.GetType().Name})");
            }

            Console.WriteLine();
            Console.WriteLine("=== Агрегація: список використаних інструментів ===");
            for (int i = 0; i < used.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {used[i]}");
            }
            Console.WriteLine($"Всього використано інструментів: {used.Count}");

            Console.ReadKey();
        }
    }
}
