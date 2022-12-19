using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using SkolPortalen.Data;
using SkolPortalen.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using System.Reflection;

namespace SkolPortalen
{
    internal class School
    {
        SchoolContext context = new SchoolContext();
        public void Menu()
        {
            int menuSelection = 0;
            while (true)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("-----------------");
                    Console.WriteLine("SkolPortalen");
                    Console.WriteLine("-----------------");
                    Console.WriteLine();
                    Console.WriteLine("#1: Avsluta Skolportalen");
                    Console.WriteLine("#2: Hämta all personal");
                    Console.WriteLine("#3: Hämta alla elever");
                    Console.WriteLine("#4: Hämta alla elever i en viss klass");
                    Console.WriteLine("#5: Hämta alla betyg från den senaste månaden");
                    Console.WriteLine("#6: Lista med alla kurser och det snittbetyg som eleverna fått på den kursen samt det högsta och lägsta betyget som någon fått i kursen");
                    Console.WriteLine("#7: Lägga till nya elever");
                    Console.WriteLine("#8: Lägg till ny personal)");
                    Console.Write("\n Välj alternativ: ");
                    try
                    {
                        menuSelection = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                }
                switch (menuSelection)
                {
                    case 1:
                        Console.WriteLine("\nAvslutar program..");
                        System.Threading.Thread.Sleep(1000);
                        Environment.Exit(0);
                        break;
                    case 2:
                        getAllEmployees();
                        break;
                    case 3:
                        getStudents();
                        break;
                    case 4:
                        getStudentsFromClass();
                        break;
                    case 5:
                        getGrades();
                        break;
                    case 6:
                        getAverageGradesAndHighestLowestGrades();
                        break;
                    case 7:
                        addStudent();
                        break;
                    case 8:
                        addEmployee();
                        break;
                }
            }
        }
        private void getAllEmployees()
        {
            string userInput = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("#1: Alla anställda");
                Console.WriteLine("#2: Lärare");
                Console.WriteLine("#3: Administratörer");
                Console.WriteLine("#0: Tillbaka till skolportalen");
                Console.Write("Välj avdelning vill du ha information från: ");
                userInput = Console.ReadLine();
                userInput = userInput.Trim();

                if (userInput == "1")
                {
                    ReadSqlTable("Select EmpFirstName, EmpLastname, TitelName , EmploymentDate From Employee, Title where FK_TitleID = Title_ID order by TitelName");
                    Console.ReadKey();
                }
                else if (userInput == "2")
                {
                    ReadSqlTable("Select EmpFirstName, EmpLastname, TitelName From Employee AS e, Title AS t where e.FK_TitleID = t.Title_ID and t.TitelName = 'Teacher'");
                    Console.ReadKey();
                }
                else if (userInput == "3")
                {
                    ReadSqlTable("Select EmpFirstName, EmpLastname, TitelName From Employee Join Title On Title_ID = FK_TitleID where TitelName = 'administrator'");
                    Console.ReadKey();
                }
                else if (userInput == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Felaktig input..");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void getStudents()
        {
            int count = 0;
            string sort = "";
            string ascendingOrDescending = "";
            var myStudents = from s in context.Students select s;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sortera efter:");
                Console.WriteLine("1#: Förnamn");
                Console.WriteLine("2#: Efternamn");
                Console.Write("Välj sortering: ");
                sort = Console.ReadLine();
                sort = sort.Trim();
                if (sort == "1" || sort == "2")
                {
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Vilken ordning:");
                Console.WriteLine("1#: Stigande"); // ascending
                Console.WriteLine("2#: Fallande"); // descending 
                Console.Write("Välj ordning: ");
                ascendingOrDescending = Console.ReadLine();
                ascendingOrDescending = ascendingOrDescending.Trim();
                if (ascendingOrDescending == "1" || ascendingOrDescending == "2")
                {
                    if (ascendingOrDescending == "1" && sort == "1")
                    {
                        myStudents = myStudents.OrderBy(s => s.StudFirstName); // Sort by Firstname in ascending order
                    }
                    else if (ascendingOrDescending == "2" && sort == "1")
                    {
                        myStudents = myStudents.OrderByDescending(s => s.StudFirstName); // Sort by Firstname in descending order
                    }
                    else if (ascendingOrDescending == "1" && sort == "2")
                    {
                        myStudents = myStudents.OrderBy(s => s.StudLastName); // Sort by Lastname in ascending order // stigande
                    }
                    else
                    {
                        myStudents = myStudents.OrderByDescending(s => s.StudLastName); // Sort by Lastname in descending order 
                    }
                    break;
                }
            }
            Console.Clear();
            foreach (var student in myStudents)
            {
                count++;
                Console.WriteLine(student.StudFirstName + " " + student.StudLastName + ". " + student.StudGender + ". " + student.StudEmail);
                Console.WriteLine("---------------------------------");
            }
            Console.WriteLine("Total: " + count + " Students");
            Console.ReadKey();
        }

        private void getStudentsFromClass()
        {
            var myStudents = from s in context.Students select s;
            var myClassrooms = from c in context.Classrooms select c;
            var myClass = from classes in context.Classes select classes;

            int count = 0;
            string sort = "";
            string ascendingOrDescending = "";
            bool classFound = false;
            string enterClass = "";
            int classroomId = 0; // saving classroom id for sorting students
            List<int> getStudentClassList = new List<int>(); // classroomID matches FK_classroomsID, then fk_studentID will be saved here for a later print out when fkstudentId = studentID.

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-------------");
            foreach (var classes in myClassrooms)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(classes.ClassroomName);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-------------");

            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Välj klass: ");
            Console.ResetColor();
            enterClass = Console.ReadLine();
            enterClass = enterClass.Trim();
            enterClass = enterClass.ToLower();

            foreach (var classes in myClassrooms)
            {
                if (enterClass == classes.ClassroomName.ToLower())
                {
                    classFound = true;
                    classroomId = classes.ClassroomId;
                    break;
                }
            }
            if (classFound == false)
            {
                Console.WriteLine("Klassen du söker finns inte registrerad..");
            }
            else
            {
                foreach (var studentClass in myClass)
                {
                    if (classroomId == studentClass.FkClassroomId)
                    {
                        getStudentClassList.Add(studentClass.FkStudentId);
                    }
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Sortera efter:");
                    Console.WriteLine("1#: Förnamn");
                    Console.WriteLine("2#: Efternamn");
                    Console.Write("Välj sortering: ");
                    sort = Console.ReadLine();
                    sort = sort.Trim();
                    if (sort == "1" || sort == "2")
                    {
                        break;
                    }
                }
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Vilken ordning:");
                    Console.WriteLine("1#: Stigande"); // ascending
                    Console.WriteLine("2#: Fallande"); // descending 
                    Console.Write("Välj ordning: ");
                    ascendingOrDescending = Console.ReadLine();
                    ascendingOrDescending = ascendingOrDescending.Trim();
                    if (ascendingOrDescending == "1" || ascendingOrDescending == "2")
                    {
                        if (ascendingOrDescending == "1" && sort == "1")
                        {
                            myStudents = myStudents.OrderBy(s => s.StudFirstName); // Sort by Firstname in ascending order
                        }
                        else if (ascendingOrDescending == "2" && sort == "1")
                        {
                            myStudents = myStudents.OrderByDescending(s => s.StudFirstName); // Sort by Firstname in descending order
                        }
                        else if (ascendingOrDescending == "1" && sort == "2")
                        {
                            myStudents = myStudents.OrderBy(s => s.StudLastName); // Sort by Lastname in ascending order // stigande
                        }
                        else
                        {
                            myStudents = myStudents.OrderByDescending(s => s.StudLastName); // Sort by Lastname in descending order 
                        }
                        break;
                    }
                }

                foreach (var classes in myClassrooms)
                {
                    if (enterClass == classes.ClassroomName.ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-----------------");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("KLASS: " + classes.ClassroomName);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-----------------");
                        break;
                    }
                }
                Console.ResetColor();
                Console.WriteLine("-------");
                foreach (var students in myStudents)
                {
                    if (getStudentClassList.Contains(students.StudentId))
                    {
                        count++;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(students.StudFirstName + " " + students.StudLastName);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.ResetColor();
                        Console.WriteLine("-------");
                    }
                }
                Console.WriteLine("Totalt: " + count + " elever");
            }
            Console.WriteLine("\nKey for menu..");
            Console.ReadKey();
        }

        private void getGrades()
        {
            Console.Clear();
            Console.WriteLine("------------------");
            Console.WriteLine("Elevens namn");
            Console.WriteLine("Kurs");
            Console.WriteLine("Betyg");
            Console.WriteLine("Signerad av");
            Console.WriteLine("Signerings datum");
            Console.WriteLine("------------------");
            ReadSqlTable("Select StudFirstName + ' ' + StudLastName AS [Student], CourseName, ExamenGrade, EmpFirstName + " +
                         "' ' + EmpLastname AS [Signed by], ExamenDate From student, Course, Employee, Examen where Student_ID" +
                         " = FK_StudentID and Course_ID = FK_CourseID and Employee_ID = examen.FK_EmployeeID and ExamenDate >= " +
                         "DATEADD(DAY, -30, getdate()) order by Student_ID");
            Console.WriteLine("\nKey for menu..");
            Console.ReadKey();
        }

        private void getAverageGradesAndHighestLowestGrades()
        {
            Console.Clear();
            Console.WriteLine("------------------");
            Console.WriteLine("Kurs");
            Console.WriteLine("Antal elever på denna kurs");
            Console.WriteLine("Snittbetyget för kursen");
            Console.WriteLine("Lägsta betyget för kursen");
            Console.WriteLine("Högsta betyget för kursen");
            Console.WriteLine("------------------");
            ReadSqlTable("Select CourseName,\r\n\tCount(FK_StudentID) AS [Antal Elever]," +
                         "AVG(ExamenGrade) AS [Snittbetyg], " +
                         "Max(ExamenGrade) AS [Lägsta Betyg]," +
                         "Min(ExamenGrade) AS [Högsta Betyg]" +
                         "  From Course " +
                         "    Join Examen On FK_CourseID = Course_ID " +
                         "      group by Course_ID, CourseName");
            Console.ReadKey();
        }

        private void addStudent()
        {
            int tempPersoNumber = 0;
            string personNUmber = "";
            string firstname = "";
            string lastname = "";
            string gender = "";
            string email = "";
            int keyTracker = 0;
            string chooseCourse = "";
            int getClassroomId = 0;

            while (true)
            {
                bool breakLoop = true;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Lägg till nya Elever");
                    Console.Write("\nElevens Personnummer (8 tecken, ej dom fyra sista): ");
                    try
                    {
                        tempPersoNumber = Convert.ToInt32(Console.ReadLine());
                        if (tempPersoNumber.ToString().Length == 8)
                        {
                            personNUmber = tempPersoNumber.ToString() + "-";
                            break;
                        }
                        else
                        {
                            Console.WriteLine("8 siffror");
                            System.Threading.Thread.Sleep(1500);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Ogiltigt format..");
                        System.Threading.Thread.Sleep(1500);
                    }
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Skriv in 4 sista siffrorna i personnumret");
                    Console.Write(personNUmber);
                    try
                    {
                        tempPersoNumber = Convert.ToInt32(Console.ReadLine());
                        if (tempPersoNumber.ToString().Length == 4)
                        {
                            personNUmber += tempPersoNumber.ToString();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("4 siffror");
                            System.Threading.Thread.Sleep(1500);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Ogiltigt format..");
                        System.Threading.Thread.Sleep(1500);
                    }
                }
                var myStudents = from Student in context.Students select Student; // loop throw students, no one should have the same personnumber
                foreach (var stud in myStudents)
                {
                    if (personNUmber == stud.StudPersonNumber)
                    {
                        breakLoop = false;
                        Console.WriteLine("Det finns redan en elev med detta personnummer, försök igen");
                        System.Threading.Thread.Sleep(1500);
                    }
                }
                if (breakLoop == true)
                {
                    break;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Personnummer: " + personNUmber);
                Console.Write("Skriv in Elevens Förnamn: ");
                firstname = Console.ReadLine();
                firstname = firstname.Trim();
                firstname = firstname.ToLower();
                if (firstname.Length < 2 || firstname.Length >= 50)
                {
                    Console.WriteLine("2 - 50 tecken");
                }
                else
                {
                    firstname = char.ToUpper(firstname[0]) + firstname.Substring(1);
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Personnummer: " + personNUmber);
                Console.WriteLine("Elevens Förnamn: " + firstname);
                Console.Write("Skriv in Elevens Efternamn: ");
                lastname = Console.ReadLine();
                lastname = lastname.Trim();
                lastname = lastname.ToLower();
                if (lastname.Length < 2 || lastname.Length >= 50)
                {
                    Console.WriteLine("2 - 50 tecken");
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    lastname = char.ToUpper(lastname[0]) + lastname.Substring(1);
                    break;
                }
            }
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Personnummer: " + personNUmber);
                Console.WriteLine("Elevens namn: " + firstname + " " + lastname);
                Console.Write("\nSkriv in Kön F/M: ");
                gender = Console.ReadLine();
                gender = gender.Trim();
                gender = gender.ToLower();
                if (gender == "m" || gender == "male" || gender == "f" || gender == "female")
                {
                    if (gender == "m" || gender == "male")
                    {
                        gender = "Male";
                    }
                    else
                    {
                        gender = "Female";
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("M för male, F för female");
                    System.Threading.Thread.Sleep(1500);
                }
            }
            Console.Clear();
            string addEmail = "";
            Console.Write("\nLägg till email? [y/n]: ");
            addEmail = Console.ReadLine();
            addEmail = addEmail.Trim();
            addEmail = addEmail.ToLower();
            if (addEmail == "y" || addEmail == "yes")
            {
                addEmail = "y";
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Personnummer: " + personNUmber);
                    Console.WriteLine("Elevens Förnamn: " + firstname);
                    Console.WriteLine("Elevens Efternamn: " + lastname);
                    Console.Write("Skriv Elevens Email: ");
                    email = Console.ReadLine();
                    email = email.Trim();
                    if (email.Length >= 50)
                    {
                        Console.WriteLine("Max 50 tecken");
                        System.Threading.Thread.Sleep(1500);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            var myClass = from classes in context.Classes select classes;  // Enter course on student
            var myCourse = from Course in context.Courses select Course;
            foreach (var classData in myClass) // gets the new studentId number and saves it in "keytracker"
            {
                if (classData.FkStudentId > keyTracker)
                {
                    keyTracker = classData.FkStudentId;
                }
            }
            keyTracker += 1; // new students fk_studentId in "Class"

            while (true)
            {
                bool breakLoop = false;
                Console.Clear();
                Console.WriteLine("--------------------");
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var course in myCourse)                       // display course-information
                {
                    Console.WriteLine("#" + course.CourseId + ": " + course.CourseName);
                }
                Console.ResetColor();
                Console.WriteLine("--------------------");
                Console.Write("\nMata in siffra för kursen eleven ska ska gå på: ");
                chooseCourse = Console.ReadLine();
                chooseCourse = chooseCourse.Trim();
                chooseCourse = chooseCourse.ToLower();
                foreach (var course in myCourse)
                {
                    if (chooseCourse == course.CourseId.ToString())
                    {
                        if (course.CourseStatus == "Inactive")
                        {
                            Console.WriteLine("Denna kurs är för närvarande inaktiv, välj någon annan kurs");
                            System.Threading.Thread.Sleep(1500);
                        }
                        else
                        {
                            breakLoop = true;
                        }
                        break;
                    }
                }
                if (breakLoop == true)
                {
                    break;
                }
            }

            switch (chooseCourse)  // due to course, classroom will automatic generates
            {
                case "1":
                    getClassroomId = 2;
                    break;
                case "2":
                    getClassroomId = 4;
                    break;
                case "3":
                    getClassroomId = 2;
                    break;
                case "4":
                    getClassroomId = 1;
                    break;
                case "5":
                    getClassroomId = 3;
                    break;
                case "6":
                    getClassroomId = 5;
                    break;
            }
            int getCourse = Convert.ToInt32(chooseCourse);
            if (addEmail == "y")
            {
                context.Students.Add(new Student(personNUmber, firstname, lastname, gender, email)); // if student has a email
            }
            else
            {
                context.Students.Add(new Student(personNUmber, firstname, lastname, gender)); // student has no email, email = NULL
            }
            context.SaveChanges();
            context.Classes.Add(new Class(keyTracker, getCourse, getClassroomId));
            context.SaveChanges();
            Console.WriteLine("Eleven har lagts till");
            System.Threading.Thread.Sleep(1500);
        }

        private void addEmployee()
        {
            string firstname = "";
            string lastname = "";
            string gender = "";
            int titleId = 0;
            int salaryId = 0;
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Lägg Till Medarbetare");
                Console.Write("Medarbetarens Förnamn: ");
                firstname = Console.ReadLine();
                firstname = firstname.Trim();
                firstname = firstname.ToLower();
                if (firstname.Length < 2 || firstname.Length >= 50)
                {
                    Console.WriteLine("2 - 50 tecken");
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    firstname = char.ToUpper(firstname[0]) + firstname.Substring(1);
                    break;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Medarbetarens Förnamn: " + firstname);
                Console.Write("Mata in Medarbetarens Efternamn: ");
                lastname = Console.ReadLine();
                lastname = lastname.Trim();
                lastname = lastname.ToLower();
                if (lastname.Length < 2 || lastname.Length >= 50)
                {
                    Console.WriteLine("2 - 50 tecken");
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    lastname = char.ToUpper(lastname[0]) + lastname.Substring(1);
                    break;
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine(firstname + " " + lastname);
                Console.Write("\nSkriv in Kön [F/M]: ");
                gender = Console.ReadLine();
                gender = gender.Trim();
                gender = gender.ToLower();
                if (gender == "m" || gender == "male" || gender == "f" || gender == "female")
                {
                    if (gender == "m" || gender == "male")
                    {
                        gender = "Male";
                    }
                    else
                    {
                        gender = "Female";
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("M för male, F för female");
                    System.Threading.Thread.Sleep(1500);
                }
            }
            Console.Clear();

            while (true)
            {
                var getTitles = from t in context.Titles select t;
                bool breakLoop = false;
                while (true)
                {
                    foreach (var titles in getTitles)
                    {
                        Console.WriteLine("ID: " + titles.TitleId + ": " + titles.TitelName);
                    }
                    Console.Write("\nEnter TitleId: ");
                    try
                    {
                        titleId = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        // do nothing
                    }
                }
                foreach (var checktitles in getTitles)
                {
                    if (checktitles.TitleId == titleId)
                    {
                        breakLoop = true;
                        break;
                    }
                }
                if (breakLoop == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid TitleId..");
                }
            }
            Console.Clear();

            while (true)
            {
                var ShowSalaryStages = from m in context.Salaries select m;
                bool breakLoop = false;
                while (true)
                {
                    foreach (var s in ShowSalaryStages)
                    {
                        Console.WriteLine("ID: " + s.SalaryId + ": " + s.SalaryStages + " kr/mån");
                    }
                    Console.Write("\nEnter SalaryId: ");
                    try
                    {
                        salaryId = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        // do nothing
                    }
                }
                foreach (var checktitles in ShowSalaryStages)
                {
                    if (checktitles.SalaryId == salaryId)
                    {
                        breakLoop = true;
                        break;
                    }
                }
                if (breakLoop == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid SalaryId..");
                }
            }
            context.Employees.Add(new Employee(firstname, lastname, gender, titleId, salaryId));
            context.SaveChanges();
            Console.WriteLine("Medarbetare har lagts till");
        }

        public static void ReadSqlTable(string sqlQuery)
        {
            SqlConnection connection = new SqlConnection(@"Data Source = DESKTOP-JN0UGIT\MSSQLSERVER01; Initial Catalog = SchoolV2; Integrated Security = true");
            connection.Open();
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---------------");
                Console.ResetColor();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine(reader[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("---------------");
                    Console.ResetColor();
                }
                connection.Close();
            }
        }
    }
}
