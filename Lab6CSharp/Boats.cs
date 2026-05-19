namespace Lab6Ships
{
    // -- Перебудувати ієрархії в лабораторної робота No5 
    // (Побудувати ієрархію класів відповідно до варіанта завдання. Згідно завдання 
    // вибрати базовий клас та похідні. В класах задати поля, які характерні для 
    // кожного класу. Для всіх класів розробити  метод Show(), який виводить дані про 
    // з визначення нових сутностей таким чином щоб базовими були 
    // декілька інтерфейсів користувача та інтерфейси .NET. ++


    interface IShowable
    {
        void Show();
    }

    interface IMovable
    {
        int Speed { get; }
        void Move();
    }

    interface IArmed
    {
        int WeaponCount { get; }
        void Attack();
    }

    abstract class Ship : IShowable, IMovable, IComparable<Ship>, ICloneable, IFormattable
    {
        protected string name;
        protected int speed;

        public string Name => name;
        public int Speed => speed;

        public Ship(string name, int speed)
        {
            this.name = name;
            this.speed = speed;
        }

        public abstract void Show();

        public void Move()
        {
            Console.WriteLine($"{name} рухається зі швидкістю {speed} вузлів.");
        }

        public int CompareTo(Ship? other)
        {
            if (other is null) return 1;
            return speed.CompareTo(other.speed);
        }

        public abstract object Clone();

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return format switch
            {
                "S" => $"{name} ({speed} вузлів)",
                "F" => ToString() ?? string.Empty,
                _ => name
            };
        }

        public override string ToString() => $"{name}, швидкість: {speed}";
    }

    class Steamship : Ship
    {
        private int power;

        public Steamship(string name, int speed, int power) : base(name, speed)
        {
            this.power = power;
        }

        public override void Show()
        {
            Console.WriteLine($"[Пароплав] Назва: {name}, Швидкість: {speed}, Потужність: {power} к.с.");
        }

        public override object Clone() => new Steamship(name, speed, power);
    }

    class Sailboat : Ship
    {
        private int sails;

        public Sailboat(string name, int speed, int sails) : base(name, speed)
        {
            this.sails = sails;
        }

        public override void Show()
        {
            Console.WriteLine($"[Вітрильник] Назва: {name}, Швидкість: {speed}, Вітрила: {sails}");
        }

        public override object Clone() => new Sailboat(name, speed, sails);
    }

    class Corvette : Ship, IArmed
    {
        private int weapon;

        public int WeaponCount => weapon;

        public Corvette(string name, int speed, int weapon) : base(name, speed)
        {
            this.weapon = weapon;
        }

        public override void Show()
        {
            Console.WriteLine($"[Корвет] Назва: {name}, Швидкість: {speed}, Зброя: {weapon}");
        }

        public void Attack()
        {
            Console.WriteLine($"{name} атакує з {weapon} гарматами.");
        }

        public override object Clone() => new Corvette(name, speed, weapon);
    }

    public class SHIPS
    {
        static public void shiptest()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Ship[] fleet =
            [
                new Steamship("A", 18, 12000),
                new Sailboat("B",          14, 3),
                new Corvette("C",        30, 8),
                new Steamship("D",        22, 8000),
                new Corvette("E",          28, 6),
            ];

            Console.WriteLine("ALL SHIPS");
            foreach (var s in fleet)
                s.Show();

            Console.WriteLine("\nIMovable.Move()");
            foreach (IMovable m in fleet)
                m.Move();

            Console.WriteLine("\nIArmed.Attack()");
            foreach (var s in fleet)
                if (s is IArmed armed)
                    armed.Attack();

            Console.WriteLine("\n IComparable ");
            Array.Sort(fleet);
            foreach (var s in fleet)
                s.Show();

            Console.WriteLine("\n ICloneable");
            Ship clone = (Ship)fleet[0].Clone();
            clone.Show();
            Console.WriteLine($"Same? {ReferenceEquals(fleet[0], clone)}");

            Console.WriteLine("\n IFormattable ");
            Console.WriteLine(fleet[0].ToString("S", null));
            Console.WriteLine(fleet[0].ToString("F", null));
        }
    }
}