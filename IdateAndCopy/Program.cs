using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Student
{
    using System.Dynamic;

    class Program
    {
        static void Main()
        {
            string s;
            Student stud_1 = new Student();

            s = stud_1.ToString();
            Console.WriteLine(s);

            Console.WriteLine((int) 3.9);
            Console.WriteLine(stud_1[Education.Bachelor] + " " + stud_1[Education.Specialist] + " " + stud_1[Education.SecondEducation]);


            DateTime dataTim = new DateTime(2002, 3, 13);


            Exam[] exams = new Exam[3];
            exams[0] = new Exam("math", 100, dataTim);
            exams[1] = new Exam("math1", 10, dataTim);
            exams[2] = new Exam("math2", 1, dataTim);
            Person tim = new Person("Temirlan","Yusupov", dataTim);
            Person tim1 = new Person("Temirlan","Yusupov", dataTim);
            if (tim.Equals(tim1) == true)
                Console.WriteLine(tim.GetHashCode() + " ==" + tim1.GetHashCode()); //Check Equals*/
            stud_1 = new Student();
            stud_1.AddExams(exams);
            stud_1.Info = tim;
            stud_1.Education = Education.Bachelor;
            stud_1.Group = 63;
            Console.WriteLine(stud_1.ToString());
            tim.YearOfbirth = 2022;
            stud_1.Info.YearOfbirth = 2022;
            Console.WriteLine(stud_1.ToString());

            Exam[] first = new Exam[1000000];
            Exam[,] two = new Exam[1000, 1000];
            Exam[][] three = new Exam[1000][];
          
            for (int i = 0; i < three.Length; i++)
                three[i] = new Exam[1000];
             var sw = Stopwatch.StartNew();

            //1
            for (int i = 0; i < 1000000; i++)
               first[i] = new Exam();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            //2
            sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                     two[i, j] = new Exam();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            
            //3
            sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            for (int j = 0; j < 1000; j++)
                three[i][j] = new Exam();

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }

    interface IdateAndCopy
    {
        DateTime Date { get; set;}
        
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
            get { return (date);}
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

            hashCode = this.Date.GetHashCode() + this.Name.GetHashCode() + this.SecondName.GetHashCode() ;
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
         public string Title
        {
            get;
            set;
        }

        public bool IsPass
        {
            get;
            set;
        }
        
    }

    class Exam : IdateAndCopy
    {
        public string Title
        {
            get;
            set;
        }

        public int Mark
        {
            get;
            set;
        }

        public System.DateTime Date_of_exam
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

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
        private List<Exam> _exam;

        //С помощью ключевого слова base можно вызвать конструктор любой формы, определяемой в базовом классе(предке)
        public Student() : base()
        {
            _education = Education.Bachelor;
            _group = 0;
            _exam = new List<Exam>();
        }

        public Student(Person person, Education education, int group)
            : base(person.Name, person.SecondName, person.Date)
        {
            this._education = _education;
            this._group = group;
            _exam = new List<Exam>();
        }


        public Person Info
        {
            get 
            {
                return ((Person)this);
            }
            set
            {
                name = value.Name;
                secondName = value.SecondName;
                date = value.Date; // или альтернатива this = value
            }
        }

        public DateTime Date
        { 
            get;
            set;
        }

        public Education Education
        {
            get
            {
                return (_education);
            }
            set
            {
                _education = value;
            }
        }

        public int Group
        {
            get
            {
                return (_group);
            }
            set
            {
                _group = value;
            }
        }

        public List<Exam> Exams
        {
            get
            {
                return (_exam);
            }
            set
            {
                _exam = value;
            }
        }

        public double AverageScore
        {
            get
            {
                int i;
                int sumScore;

                for (i = 0, sumScore = 0; i < _exam.Count; i++, sumScore += _exam[i].Mark)
                    ;
                if (i == 0)
                    return (i);
                return ((double)sumScore / i);
            }
        }


        public bool this[Education i] => i == _education;

        public void AddExams(Exam[] exams)
        {
            exams.ToList();
            this._exam.AddRange(exams);
        }

        public override string ToString()
        {
            string s;

            s = base.ToString() + " " + _education.ToString() + " " + _group.ToString();
            foreach (var temp in _exam)
                s += " " + temp.Title;

            return (s);
        }

        public virtual string ToShortString()
        {
            string s;

            s = base.ToString() + " " + _education.ToString() + " " + _group.ToString();
            s += AverageScore.ToString();

            return (s);
        }

        public virtual object DeepCopy()
        {
            return (1);
        }
    }

}
