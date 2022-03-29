using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Dynamic;


namespace Student
{

    class Program
    {
        static void Main()
        {
            Person first = new Person();
            Person second = new Person();
            Console.WriteLine("Сравнение :\n1) На совпадение ссылок: " + ReferenceEquals(first, second));
            Console.WriteLine("2) Хеш коды: " + first.GetHashCode() +  second.GetHashCode()); // bcghfdbnm
            
            DateTime summer = new DateTime(2022, 6, 13);
            Exam exam1 = new Exam("math", 99, summer);
            Exam exam2 = new Exam("bzd", 99, summer);
            Exam exam3 = new Exam("algem", 99, summer);
            Test test1 = new Test("colloquim", true);
            Test test2 = new Test("math", true);
            Test test3 = new Test("bzd", true);

            Student ivan = new Student(first, Education.Bachelor, 123);
            ivan.AddExams(exam1, exam2, exam3);
            ivan.AddTest(test1, test2, test3);
            Person temp = ivan.Info; // 3
            Console.WriteLine(temp); // 3
            var maxim = ivan.DeepCopy();
            ivan.AddExams(exam3, exam1);
            
            Console.WriteLine("4)Check Deep copy\n" + ivan.ToString() + "\n"+ maxim.ToString() + "\n"); //4
            
            Console.Write("5)");
            try // 5
            {
                ivan.Group = 700232;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("6) all exams and tests : ");
            foreach (var going in ivan.GetExamTest())
            {
                if (going.GetType() == exam1.GetType())
                    Console.Write(" " + ((Exam)going).Title);
                else
                    Console.Write(" " + ((Test)going).Title);
            }
            Console.WriteLine("\n7) all exams with points > 3");
            foreach (Exam going in ivan.GetExamMoreThan(3))
            {
                Console.Write(" " + going.Title);
            }
            
            Console.WriteLine("\n8) all  tests.title == exam.title");
            foreach (string title in ivan)
                Console.Write(" " + title);
            Console.WriteLine("||");
            foreach (string title in ivan)
                Console.Write(" " + title);
            Console.WriteLine("\n9) all passed exams and tests");
            foreach (var going in ivan.GetExTestPass())
            {
                if (going.GetType() == exam1.GetType())
                    Console.Write(" " + ((Exam)going).Title);
                else
                    Console.Write(" " + ((Test)going).Title);
            }
            Console.WriteLine("\n10) all tests with passed exams");
            foreach (Test going in ivan.GetTestWithPassEx())
                Console.Write(" " + going.Title);
            Student timur = new Student();
            timur.AddExams(exam1, exam2, exam3);
            Student ahad = (Student)timur.DeepCopy();
            ((Exam) (ahad.Exams[0])).Mark = 55;
            Console.WriteLine("\nTImur and Ahad"  + timur.ToString() + "\n" + ahad.ToString());
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

            if (obj == null)
                return (false);
            if (temp.GetType() != this.GetType())
                return (false);
            return (temp.Name == this.Name && temp.SecondName == this.secondName && temp.Date == this.Date);
         ;
        }

        public static bool operator ==(Person a, Person b) => a.Equals(b);

        public static bool operator !=(Person a, Person b) => !(a == b);
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
            return (new Exam(Title, Mark, Date));
        }
    }

    class Student : Person, IdateAndCopy, IEnumerable
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
            _test = new ArrayList();
        }

        public Student(Person person, Education education, int group)
            : base(person.Name, person.SecondName, person.Date)
        {
            this._education = _education;
            this._group = group;
            _exam = new ArrayList();
            _test = new ArrayList();

        }


        public Person Info
        {
            get {return (new Person(name, secondName, date));}
            set
            {
                name = value.Name;
                secondName = value.SecondName;
                date = value.Date; // или альтернатива this = value
            }
        }

        public DateTime Date
        {
            get => date;
            set { date = value.Date; }
        }

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
                    throw new Exception("Group is not valid. Enter in range [100, 599]");
            }
        }

        public ArrayList Exams
        {
            get { return (_exam); }
            set { _exam = value; }
        }

        public ArrayList Test
        {
            get => _test;
            set { _test = value; }
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
            foreach (var temp in exams)
                _exam.Add(temp);
        }

        public void AddTest(params Test[] test)
        {
            foreach (var temp in test)
                _test.Add(temp);
        }

        public override string ToString()
        {
            string s;
            // stringbuilder
            s = base.ToString() + " " + _education.ToString() + " " + _group.ToString();
            foreach (Exam temp in _exam)
                s += " " + temp.ToString();
            foreach (Test temp in _test)
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

        public object DeepCopy() // переделать
        {
            Person a = new Person(name, secondName, date);
            Student temp = new Student(a, _education, _group);
            ArrayList examines1 = new ArrayList();
            ArrayList test1 = new ArrayList();

            foreach (Exam going in _exam)
            {
                Exam temper = (Exam)going.DeepCopy();
                examines1.Add(temper);
            }

            foreach (Test going in _test)
            {
                Test temper = new Test(going.Title, going.IsPass);
                test1.Add(going);
            }

            temp.Exams = examines1;
            temp.Test = test1;
            return (temp);
        }

        public IEnumerable GetExamTest()
        {
            foreach (object temp in _exam)
                yield return temp;
            foreach (object temp in _test)
                yield return temp;      
        }

        public IEnumerable GetExamMoreThan(int mark)
        {
            foreach (Exam going in _exam)
                if (going.Mark > mark)
                    yield return going;
        }

        public IEnumerable GetExTestPass()
        {
            foreach (object temp in _exam)
                if (((Exam) temp).Mark > 2)
                    yield return temp;
            foreach (Test going in _test)
                if (going.IsPass == true)
                    yield return going;
        }

        public IEnumerable GetTestWithPassEx()
        {
            foreach (Test goTest in _test)
                foreach (Exam goEx in _exam)
                    if (goTest.Title == goEx.Title && goEx.Mark > 2)
                        yield return goTest;
        }
        
        public IEnumerator GetEnumerator() => new StudentEnumerator(_exam, _test);

        public class StudentEnumerator : IEnumerator
        {
            private int position = -1;
            private ArrayList listExTest;
            
            public StudentEnumerator(ArrayList examines, ArrayList tests)
            {
                listExTest = new ArrayList();
                foreach (Exam temp in examines)
                    foreach (Test temp1 in tests)
                        if (temp.Title == temp1.Title)
                            listExTest.Add(temp.Title);
            }
            public object Current
            {
                get
                {
                    if (position == -1 || position >= listExTest.Count)
                        throw new ArgumentException();
                    return listExTest[position];
                }
            }
            public bool MoveNext()
            {
                if (position < listExTest.Count - 1)
                {
                    position++;
                    return true;
                }
                else
                    return false;
            }
            public void Reset() => position = -1;
        }
        
    }
}