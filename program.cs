using System;

public class Program
{
    public static void Main()
    {
        Robot robot1 = new RobotNormal("RobotAlpha", 100, 15, 20);
        Robot boss = new BossRobot("MegaBoss", 300, 50, 40);

        IKemampuan perbaikan = new Perbaikan();
        IKemampuan seranganListrik = new SeranganListrik();
        IKemampuan seranganPlasma = new SeranganPlasma();
        IKemampuan pertahananSuper = new PertahananSuper();

        robot1.GunakanKemampuan(seranganPlasma);
        boss.Serang(robot1);

        boss.GunakanKemampuan(seranganListrik);
        robot1.Serang(boss);

        boss.GunakanKemampuan(pertahananSuper);

        robot1.GunakanKemampuan(perbaikan);
        robot1.Serang(boss);

        robot1.CetakInformasi();
        boss.CetakInformasi();
    }
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        Console.WriteLine($"{Nama} menyerang {target.Nama} dengan {damage} damage.");
        target.Energi -= damage;
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

public class BossRobot : Robot
{
    public int Pertahanan { get; set; }

    public BossRobot(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor + 20, serangan) // Armor bos lebih besar
    {
        Pertahanan = 20;
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - Armor;
        if (damage < 0) damage = 0;
        Energi -= damage;
        Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama} dengan {damage} damage.");

        if (Energi <= 0)
        {
            Mati();
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah kalah dalam pertarungan.");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        kemampuan.Gunakan(this);
    }
}

public class RobotNormal : Robot
{
    public RobotNormal(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        kemampuan.Gunakan(this);
    }
}

public interface IKemampuan
{
    void Gunakan(Robot target);
    bool OnCooldown { get; }
}

public class Perbaikan : IKemampuan
{
    public bool OnCooldown { get; private set; }

    public void Gunakan(Robot target)
    {
        if (!OnCooldown)
        {
            target.Energi += 30;
            OnCooldown = true;
            Console.WriteLine($"{target.Nama} menggunakan Perbaikan, energi bertambah 30.");
        }
        else
        {
            Console.WriteLine("Kemampuan Perbaikan sedang cooldown.");
        }
    }
}

public class SeranganListrik : IKemampuan
{
    public bool OnCooldown { get; private set; }

    public void Gunakan(Robot target)
    {
        if (!OnCooldown)
        {
            target.Energi -= 40;
            target.Armor = 0; 
            OnCooldown = true;
            Console.WriteLine($"{target.Nama} terkena Serangan Listrik, energi berkurang 40 dan armor menjadi 0.");
        }
        else
        {
            Console.WriteLine("Serangan Listrik sedang cooldown.");
        }
    }
}

public class SeranganPlasma : IKemampuan
{
    public bool OnCooldown { get; private set; }

    public void Gunakan(Robot target)
    {
        if (!OnCooldown)
        {
            target.Energi -= 50;
            OnCooldown = true;
            Console.WriteLine($"{target.Nama} terkena Serangan Plasma, energi berkurang 50.");
        }
        else
        {
            Console.WriteLine("Serangan Plasma sedang cooldown.");
        }
    }
}

public class PertahananSuper : IKemampuan
{
    public bool OnCooldown { get; private set; }

    public void Gunakan(Robot target)
    {
        if (!OnCooldown)
        {
            target.Armor += 20;
            OnCooldown = true;
            Console.WriteLine($"{target.Nama} menggunakan Pertahanan Super, armor bertambah 20 poin.");
        }
        else
        {
            Console.WriteLine("Pertahanan Super sedang cooldown.");
        }
    }
}
