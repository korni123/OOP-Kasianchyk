using System;

namespace lab6v9
{
    public class Weapon
    {
        private string name;
        private int damage;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public Weapon(string name, int damage)
        {
            this.name = name;
            this.damage = damage;
        }

        public virtual void Attack()
        {
            Console.WriteLine($"{Name} завдає {Damage} одиниць шкоди.");
        }

        public string GetWeaponType()
        {
            return "Зброя (Weapon)";
        }
    }

    public class Sword : Weapon
    {
        private string material;

        public string Material
        {
            get { return material; }
            set { material = value; }
        }

        public Sword(string name, int damage, string material) : base(name, damage)
        {
            this.material = material;
        }

        public override void Attack()
        {
            Console.WriteLine($"Меч {Name} ({Material}) робить рубаний удар і завдає {Damage} шкоди.");
        }

        public void Parry()
        {
            Console.WriteLine($"Меч {Name} парирує удар противника.");
        }

        public new string GetWeaponType()
        {
            return "Меч (Sword) — холодна зброя";
        }
    }

    public class Bow : Weapon
    {
        private string arrowType;

        public string ArrowType
        {
            get { return arrowType; }
            set { arrowType = value; }
        }

        public Bow(string name, int damage, string arrowType) : base(name, damage)
        {
            this.arrowType = arrowType;
        }

        public override void Attack()
        {
            base.Attack();
            Console.WriteLine($"Лук {Name} випускає стрілу типу \"{ArrowType}\".");
        }

        public void Aim()
        {
            Console.WriteLine($"Лучник прицілюється з лука {Name}...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Weapon club = new Weapon("Дубина", 10);
            Sword sword = new Sword("Екскалібур", 50, "сталь");
            Bow bow = new Bow("Довгий лук", 35, "вогняна");

            Console.WriteLine("=== 1. Власні методи похідних класів ===");
            sword.Parry();
            bow.Aim();

            Console.WriteLine();
            Console.WriteLine("=== 2. Поліморфізм: виклик Attack() через посилання типу Weapon ===");
            Weapon[] arsenal = { club, sword, bow };
            foreach (Weapon w in arsenal)
            {
                w.Attack();
            }

            Console.WriteLine();
            Console.WriteLine("=== 3. override: тип посилання НЕ впливає на результат ===");
            Weapon weaponRef = sword;
            Console.Write("sword.Attack()     -> ");
            sword.Attack();
            Console.Write("weaponRef.Attack() -> ");
            weaponRef.Attack();

            Console.WriteLine();
            Console.WriteLine("=== 4. new: результат залежить від типу посилання ===");
            Console.WriteLine($"sword.GetWeaponType()     -> {sword.GetWeaponType()}");
            Console.WriteLine($"weaponRef.GetWeaponType() -> {weaponRef.GetWeaponType()}");
            Console.WriteLine($"((Sword)weaponRef).GetWeaponType() -> {((Sword)weaponRef).GetWeaponType()}");

            Console.WriteLine();
            Console.WriteLine("=== 5. Властивості ===");
            sword.Damage = 60;
            Console.WriteLine($"Після покращення: {sword.Name}, шкода = {sword.Damage}, матеріал = {sword.Material}");

            Console.ReadKey();
        }
    }
}