namespace Lab6Persona
{

    // --    Створити інтерфейс  Persona  з методами,  що  дозволяють вивести на 
    // екран інформацію про персону, а також визначити її вік (на момент поточної 
    // дати). Створити похідні класи: Абітурієнт (прізвище, дата народження, 
    // факультет),  Студент  (прізвище,  дата  народження,  факультет,  курс), 
    // Викладати  (прізвище,  дата  народження,  факультет,  посада,  стаж),  зі 
    // своїми методами висновку інформації на екран, і визначення віку. Створити 
    // базу (масив) з n персон, вивести повну інформацію з бази на екран, а також 
    // організувати пошук персон, чий вік попадає в заданий діапазон. ++

    interface IPersona : IComparable<IPersona>, ICloneable
    {
        void Show();
        int Age();
    }

    class Applicant : IPersona
    {
        string lastName;
        DateOnly birthDate;
        string faculty;

        public Applicant(string lastName, DateOnly birthDate, string faculty)
        {
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.faculty = faculty;
        }

        public int Age()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public void Show()
        {
            Console.WriteLine($"[Applicant] {lastName}, age: {Age()}, corpse: {faculty}");
        }

        public int CompareTo(IPersona? other) => Age().CompareTo(other?.Age());
        public object Clone() => new Applicant(lastName, birthDate, faculty);
    }

    class Student : IPersona
    {
        string lastName;
        DateOnly birthDate;
        string faculty;
        int course;

        public Student(string lastName, DateOnly birthDate, string faculty, int course)
        {
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.faculty = faculty;
            this.course = course;
        }

        public int Age()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public void Show()
        {
            Console.WriteLine($"[Student]    {lastName}, вік: {Age()}, corps: {faculty}, course: {course}");
        }

        public int CompareTo(IPersona? other) => Age().CompareTo(other?.Age());
        public object Clone() => new Student(lastName, birthDate, faculty, course);
    }

    class Teacher : IPersona
    {
        string lastName;
        DateOnly birthDate;
        string faculty;
        string position;
        int experience;

        public Teacher(string lastName, DateOnly birthDate, string faculty, string position, int experience)
        {
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.faculty = faculty;
            this.position = position;
            this.experience = experience;
        }

        public int Age()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public void Show()
        {
            Console.WriteLine($"[Teacher]   {lastName}, age: {Age()}, corpse: {faculty}, place: {position}, years: {experience}");
        }

        public int CompareTo(IPersona? other) => Age().CompareTo(other?.Age());
        public object Clone() => new Teacher(lastName, birthDate, faculty, position, experience);
    }

    public class PersonaTest
    {
        static public void persontest()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IPersona[] db =
            [
                new Applicant("Коваленко", new DateOnly(2006, 5, 12),  "CS"),
                new Applicant("Мельник",   new DateOnly(2007, 3, 22),  "Math"),
                new Student(  "Бондаренко",new DateOnly(2004, 8, 1),   "Physics",            2),
                new Student(  "Шевченко",  new DateOnly(2003, 11, 30), "CS", 3),
                new Student(  "Кравченко", new DateOnly(2002, 6, 15),  "Chemystry",             4),
                new Teacher(  "Іваненко",  new DateOnly(1975, 4, 20),  "Math",        "Доцент",   18),
                new Teacher(  "Петренко",  new DateOnly(1968, 9, 5),   "Phys",            "Професор", 30),
                new Teacher(  "Сидоренко", new DateOnly(1990, 1, 17),  "CS", "Асистент",  5),
            ];

            Console.WriteLine(" All persons ");
            foreach (var p in db)
                p.Show();

            Console.WriteLine("\n Sort by age (IComparable) ");
            Array.Sort(db);
            foreach (var p in db)
                p.Show();

            Console.WriteLine("\n 18 to 25 ");
            foreach (var p in db)
                if (p.Age() >= 18 && p.Age() <= 25)
                    p.Show();

            Console.WriteLine("\n (ICloneable) ");
            var clone = (IPersona)db[0].Clone();
            clone.Show();
            Console.WriteLine($"Same? {ReferenceEquals(db[0], clone)}");
        }
    }
}