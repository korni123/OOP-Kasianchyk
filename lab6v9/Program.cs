using System;

public class Weapon
{
    private string _name;
    private int _damage;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public Weapon(string name, int damage)
    {
        _name = name;
        _damage = damage;
    }

    public virtual void Attack()
    {
        Console.WriteLine($"{_name} завдає удару на {_damage} одиниць шкоди.");
    }

    public string GetWeaponType()
    {
        return "Базова зброя";
    }
}

public class Sword : Weapon
{
    public string Material { get; set; }

    public Sword(string name, int damage, string material)
        : base(name, damage)
    {
        Material = material;
    }

    public override void Attack()
    {
        Console.WriteLine($"{Name} (меч з {Material}) рубає, завдаючи {Damage} шкоди.");
    }

    public void Parry()
    {
        Console.WriteLine($"{Name} парирує удар супротивника.");
    }

    // Демонстрація приховування через new (не override)
    public new string GetWeaponType()
    {
        return "Холодна зброя (меч)";
    }
}

public class Bow : Weapon
{
    public string ArrowType { get; set; }

    public Bow(string name, int damage, string arrowType)
        : base(name, damage)
    {
        ArrowType = arrowType;
    }

    public override void Attack()
    {
        Console.WriteLine($"{Name} стріляє стрілою типу \"{ArrowType}\", завдаючи {Damage} шкоди.");
    }

    public void Aim()
    {
        Console.WriteLine($"{Name} прицілюється перед пострілом.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Поліморфна поведінка через посилання на базовий клас ---");
        Weapon[] weapons = new Weapon[]
        {
            new Weapon("Звичайна зброя", 5),
            new Sword("Ексалібур", 15, "сталі"),
            new Bow("Довгий лук", 10, "вогняна стріла")
        };

        foreach (var w in weapons)
        {
            w.Attack(); // викликається перевизначена версія завдяки override (поліморфізм)
        }

        Console.WriteLine("\n--- Унікальні методи похідних класів ---");
        Sword sword = new Sword("Катана", 12, "дамаської сталі");
        Bow bow = new Bow("Арбалет", 8, "сталева стріла");
        sword.Parry();
        bow.Aim();

        Console.WriteLine("\n--- Різниця між override та new ---");
        Sword sword2 = new Sword("Меч воїна", 10, "бронзи");
        Weapon weaponRef = sword2; // те саме фізично, але посилання типу Weapon

        Console.WriteLine($"Через Sword-посилання: {sword2.GetWeaponType()}");   // new-версія (Sword)
        Console.WriteLine($"Через Weapon-посилання: {weaponRef.GetWeaponType()}"); // базова версія (Weapon)!

        Console.WriteLine("\nПорівняння: Attack() через Weapon-посилання викликає Sword-версію (override),");
        Console.WriteLine("а GetWeaponType() через те саме посилання викликає Weapon-версію (new, не override).");
        weaponRef.Attack();
    }
}