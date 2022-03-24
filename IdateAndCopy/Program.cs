using System;
using System.Collections;
using System.Diagnostics;

namespace Student
{
    using System.Dynamic;

    class Program
    {
        static void Main()
        {
            Person first = new Person();
            Person second = new Person();
            Console.WriteLine("Сравнение :\n 1) На совпадение ссылок: " + ((object)first == (object)second));
            Console.WriteLine("2) Хеш коды: " + first.GetHashCode() + " == " + second.GetHashCode());
        }
    }

    interface IdateAndCopy
    {
        DateTime Date { get; set; }

        object DeepCopy();
    }

    class Person : IdateAndCopy
    {
        protected string name;
        protected string secondName;
        protected System.DateTime date;

        public Person(string a, string b, DateTime date)
        {
            this.name = a;
            this.secondName = b;
            this.date = date;
        }

        public Person()
        {
            this.name = "Ivan";
            this.secondName = "Ivanov";
            this.date = new DateTime();
        }

        public string Name
        {
            get { return (name); }
            set { this.name = value; }
        }

        public string SecondName
        {
            get { return (secondName); }
            set { this.secondName = value; }
        }

        public int YearOfbirth
        {
            get { return ((date.Year)); }
            set { date = date.AddYears(-date.Year + value); }
        }

        public DateTime Date
        {
            get { return (date); }
            set { date = value; }
        }

        public override string ToString()
        {
            string all_information;

            all_information = name + " " + secondName + " " + date.ToString();
            return (all_information);
        }

        public virtual string ToShortString()
        {
            string fullname;

            fullname = name + secondName;
            return (fullname);
        }

        public override bool Equals(object obj)
        {
            Person temp = (Person) obj;

            if (obj.GetType() != this.GetType())
                return (false);
            if (temp.Name == this.Name && temp.SecondName == this.secondName && temp.Date == this.Date)
                return (true);
            return (false);
        }

        public static bool operator ==(Person a, Person b)
        {
            if (a.Equals(b) == true)
                return (true);
            return (false);
        }

        public static bool operator !=(Person a, Person b)
        {
            if (a.Equals(b) == true)
                return (false);
            return (true);
        }

        public override int GetHashCode()
        {
            int hashCode;

            hashCode = Date.GetHashCode() + Name.GetHashCode() + SecondName.GetHashCode();
            return (hashCode);
        }

        public virtual object DeepCopy()
        {
            Person temp = new Person(name, secondName, date);

            return (temp);

        }
    }

    enum Education
    {
        Specialist,
        Bachelor,
        SecondEducation
    }

    class Test
    {

        public Test()
        {
            Title = "math";
            IsPass = true;
        }

        public Test(string name, bool isPassed)
        {
            Title = name;
            IsPass = isPassed;
        }

        public override string ToString()
        {
            return (Title + " " + IsPass);
        }

        public string Title { get; set; }

        public bool IsPass { get; set; }

    }

    class Exam : IdateAndCopy
    {
        public string Title { get; set; }

        public int Mark { get; set; }

        public System.DateTime Date_of_exam { get; set; }

        public DateTime Date { get; set; }

        public Exam()
        {
            this.Title = "Subject";
            this.Mark = 0;
            this.Date_of_exam = new DateTime();
        }

        public Exam(string subj, int n, DateTime date)
        {
            this.Title = subj;
            this.Mark = n;
            this.Date_of_exam = date;
        }

        public override string ToString()
        {
            return (Title + " " + Mark + " " + Date_of_exam.ToString());
        }

        public object DeepCopy()
        {
            return (1);
        }
    }

    class Student : Person, IdateAndCopy
    {
        private Education _education;
        private int _group;
        private ArrayList _exam;
        private ArrayList _test;

        //С помощью ключевого слова base можно вызвать конструктор любой формы, определяемой в базовом классе(предке)
        public Student() : base()
        {
            _education = Education.Bachelor;
            _group = 0;
            _exam = new ArrayList();
        }

        public Student(Person person, Education education, int group)
            : base(person.Name, person.SecondName, person.Date)
        {
            this._education = _education;
            this._group = group;
            _exam = new ArrayList();
        }


        public Person Info
        {
            get { return ((Person) this); }
            set
            {
                name = value.Name;
                secondName = value.SecondName;
                date = value.Date; // или альтернатива this = value
            }
        }

        public DateTime Date { get; set; }

        public Education Education
        {
            get { return (_education); }
            set { _education = value; }
        }

        public int Group
        {
            get { return (_group); }
            set
            {
                if (value <= 599 && value > 100)
                    _group = value;
                else
                    throw new ArgumentOutOfRangeException("Group is not valid");
            }
        }

        public ArrayList Exams
        {
            get { return (_exam); }
            set { _exam = value; }
        }

        public double AverageScore
        {
            get
            {
                int i;
                int sumScore;

                sumScore = 0;
                Exam temp;
                for (i = 0, sumScore = 0; i < _exam.Count; i++, sumScore += ((Exam)(_exam[i])).Mark)
                    ;
                if (i == 0)
                    return (i);
                return ((double) sumScore / i);
            }
        }


        public bool this[Education i] => i == _education;

        public void AddExams(params Exam[] exams)
        {
            exams.ToList();
            this._exam.AddRange(exams);
        }

        public override string ToString()
        {
            string s;

            s = base.ToString() + " " + _education.ToString() + " " + _group.ToString();
            foreach (Exam temp in _exam)
                s += " " + temp.Title;

            return (s);
        }

        public virtual string ToShortString()
        {
            string s;

            s = base.ToString() + " " + _education.ToString() + " " + _group.ToString() + " ";
            s += AverageScore.ToString();

            return (s);
        }

        public object DeepCopy()
        {
            Person a = new Person(name, secondName, date);
            Student temp = new Student(a, _education, _group);
            ArrayList examines1 = new ArrayList();
            ArrayList test1 = new ArrayList();

            foreach (var going in _exam)
                examines1.Add(going);
            foreach (var going in _exam)
                test1.Add(going);
            temp.Exams = examines1;
            return (temp);
        }

        public IEnumerable GetExamTest()
        {
            foreach (Exam temp in _exam)
                yield return temp;
            foreach (Exam temp in _test)
                yield return temp;            
        }

        public IEnumerable GetExamMoreThan(int mark)
        {
            foreach (Exam going in _exam)
                if (going.Mark > mark)
                    yield return going;
        }
        
    }
}
