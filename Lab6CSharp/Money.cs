using System.Collections;

namespace Lab6Money
{

    // -- Реалізувати  обробку  помилок  для  однієї  із  задач  з 
    // попередніх  лабораторних  робот,  при  цьому  перевизначивши  за  допомогою 
    // спадкування класу Exception  власні класи винятків, також реалізувати обробку 
    // стандартних винятків: IndexOutOfRangeException 
    // До одного з розроблених класів в лабораторної робота 
    // No4 або No5 добавити стандартні інтерфейси .NET перерахування щоб можна 
    // застосовувати оператор foreach. ++

    public class MoneyException : Exception
    {
        public MoneyException(string message) : base(message) { }
        public MoneyException(string message, Exception inner) : base(message, inner) { }
    }

    public class NegativeValueException : MoneyException
    {
        public int Value { get; }
        public NegativeValueException(string field, int value)
            : base($"'{field}' cant be negative . Get: {value}")
        {
            Value = value;
        }
    }

    public class ZeroPriceException : MoneyException
    {
        public ZeroPriceException()
            : base("Cant be negative.") { }
    }

    public class InsufficientFundsException : MoneyException
    {
        public int Total { get; }
        public int Price { get; }
        public InsufficientFundsException(int total, int price)
            : base($"Not enought money: є {total}, needed {price}.")
        {
            Total = total;
            Price = price;
        }
    }

    public class Money : IEnumerable<int>
    {
        protected int nominal;
        protected int num;

        public Money(int nominal, int num)
        {
            if (nominal < 0) throw new NegativeValueException(nameof(nominal), nominal);
            if (num < 0) throw new NegativeValueException(nameof(num), num);
            this.nominal = nominal;
            this.num = num;
        }

        public int Nominal
        {
            get => nominal;
            set
            {
                if (value < 0) throw new NegativeValueException(nameof(Nominal), value);
                nominal = value;
            }
        }

        public int Num
        {
            get => num;
            set
            {
                if (value < 0) throw new NegativeValueException(nameof(Num), value);
                num = value;
            }
        }

        public int Total => nominal * num;

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => nominal,
                    1 => num,
                    _ => throw new IndexOutOfRangeException(
                             $"Index {index} out of (0..1).")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: Nominal = value; break;
                    case 1: Num = value; break;
                    default:
                        throw new IndexOutOfRangeException(
                            $"Index {index} out of (0..1).");
                }
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            yield return nominal;
            yield return num;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Money operator ++(Money m)
        {
            m.nominal++;
            m.num++;
            return m;
        }

        public static Money operator --(Money m)
        {
            if (m.nominal - 1 < 0) throw new NegativeValueException("nominal", m.nominal - 1);
            if (m.num - 1 < 0) throw new NegativeValueException("num", m.num - 1);
            m.nominal--;
            m.num--;
            return m;
        }

        public static bool operator !(Money m) => m.num != 0;

        public static Money operator +(Money m, int scalar)
        {
            if (m.num + scalar < 0) throw new NegativeValueException("num", m.num + scalar);
            return new Money(m.nominal, m.num + scalar);
        }

        public static Money operator +(int scalar, Money m) => m + scalar;

        public static implicit operator string(Money m) =>
            $"Nominal={m.nominal}, Num={m.num}, Total={m.Total}";

        public static explicit operator Money(string s)
        {
            var parts = s.Split(':');
            if (parts.Length != 2
                || !int.TryParse(parts[0], out int n)
                || !int.TryParse(parts[1], out int k))
            {
                throw new FormatException(
                    "Рядок має бути у форматі \"nominal:num\" (наприклад, \"10:5\").");
            }
            return new Money(n, k);
        }

        public void Print()
        {
            Console.WriteLine($"Номінал: {nominal}, Кількість: {num}, Сума: {Total}");
        }

        public bool CanBuy(int price)
        {
            if (price <= 0) throw new ZeroPriceException();
            return Total >= price;
        }

        public int CountItems(int price)
        {
            if (price <= 0) throw new ZeroPriceException();
            return Total / price;
        }
    }

    public class Moneytest
    {
        static public void Demo(string label, Action action)
        {
            Console.WriteLine($"\n--- {label} ---");
            try
            {
                action();
            }
            catch (NegativeValueException ex) { Console.WriteLine($"[NegativeValueException] {ex.Message}"); }
            catch (ZeroPriceException ex) { Console.WriteLine($"[ZeroPriceException] {ex.Message}"); }
            catch (InsufficientFundsException ex) { Console.WriteLine($"[InsufficientFundsException] {ex.Message}"); }
            catch (MoneyException ex) { Console.WriteLine($"[MoneyException] {ex.Message}"); }
            catch (IndexOutOfRangeException ex) { Console.WriteLine($"[IndexOutOfRangeException] {ex.Message}"); }
            catch (FormatException ex) { Console.WriteLine($"[FormatException] {ex.Message}"); }
            catch (ArrayTypeMismatchException ex) { Console.WriteLine($"[ArrayTypeMismatchException] {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"[Exception] {ex.Message}"); }
        }

        static public void moneytest()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Demo("Normal + foreach", () =>
            {
                var m = new Money(10, 5);
                m.Print();
                foreach (int val in m)
                    Console.WriteLine($"  поле = {val}");
            });

            Demo("LINQ Money", () =>
            {
                var m = new Money(10, 5);
                Console.WriteLine($"  Sum: {m.Sum()}, Max: {m.Max()}, Min: {m.Min()}");
            });

            Demo("foreach Money[]", () =>
            {
                Money[] arr = [new Money(10, 3), new Money(5, 7), new Money(2, 10)];
                foreach (var item in arr)
                    item.Print();
            });

            Demo("Negative", () =>
            {
                var m = new Money(-5, 3);
            });

            Demo("Negative num cuz of bla bla", () =>
            {
                var m = new Money(10, 5);
                m.Num = -1;
            });

            Demo("Wrong index", () =>
            {
                var m = new Money(10, 5);
                _ = m[5];
            });

            Demo("-- cause negative", () =>
            {
                var m = new Money(0, 0);
                m--;
            });

            Demo("Null price CanBuy", () =>
            {
                var m = new Money(10, 5);
                m.CanBuy(0);
            });

            Demo("InsufficientFundsException", () =>
            {
                var m = new Money(5, 2);
                if (!m.CanBuy(100))
                    throw new InsufficientFundsException(m.Total, 100);
            });

            Demo("Wrong format number", () =>
            {
                var m = (Money)"wrong";
            });

            Demo("ArrayTypeMismatchException", () =>
            {
                object[] objs = new string[3];
                objs[0] = new Money(1, 1);
            });
        }
    }
}