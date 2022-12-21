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
using static System.Net.Mime.MediaTypeNames;

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
                    Console.WriteLine("#9: Hur många anställda jobbar på de olika avdelningarna "); // EF i VS"
                    Console.WriteLine("#10: Visa All information om alla elever ");
                    Console.WriteLine("#11: Visa en lista på alla aktiva kurser ");
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
                    case 9:
                        CountAllEMployee();
                        break;
                    case 10:
                        allDataInStudent();
                        break;
                    case 11:
                        displayCourseStatus();
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
            int keyTrackerForPhone = 0; //get the upcoming Student_ID
            int keyTracker = 0; // get the upcoming FK_studentID
            string email = "";
            string addEmail = "";
            int getClassroomId = 0;

            string personNumber = getPersonNumber();
            string firstname = getFirstname();
            string lastname = getLastname();
            string gender = getGender();
            string phoneNumber = getPhoneDetails();
            email = getEmail();
            int getCourse = getCourseAndClass();
            addStudentToDatabase();


            // ---Local funktions---
            string getPersonNumber()
            {
                int tempPersonNumber = 0;
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
                            tempPersonNumber = Convert.ToInt32(Console.ReadLine());
                            if (tempPersonNumber.ToString().Length == 8)
                            {
                                personNumber = tempPersonNumber.ToString() + "-";
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
                        Console.Write(personNumber);
                        try
                        {
                            tempPersonNumber = Convert.ToInt32(Console.ReadLine());
                            if (tempPersonNumber.ToString().Length == 4)
                            {
                                personNumber += tempPersonNumber.ToString();
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
                        if (personNumber == stud.StudPersonNumber)
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
                return personNumber;
            }

            string getFirstname()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Personnummer: " + personNumber);
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
                return firstname;
            }

            string getLastname()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Personnummer: " + personNumber);
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
                return lastname;
            }

            string getGender()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Personnummer: " + personNumber);
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
                return gender;
            }

            string getPhoneDetails()
            {
                while (true)
                {
                    int tempPhoneNumber = 0;
                    bool breakLoop = false;

                    var myStudents = from s in context.Students select s;
                    myStudents = myStudents.OrderByDescending(s => s.StudentId);

                    while (true)
                    {
                        Console.Clear();
                        Console.Write("Ange mobilnnummer utan mellanslag: ");
                        try
                        {
                            tempPhoneNumber = Convert.ToInt32(Console.ReadLine());

                            if (tempPhoneNumber.ToString().Length == 9)
                            {
                                breakLoop = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Ogiltigt format, försök igen");
                                System.Threading.Thread.Sleep(1500);
                            }
                        }
                        catch (Exception)
                        {
                            // do nothing
                        }
                    }

                    phoneNumber = tempPhoneNumber.ToString();
                    phoneNumber = phoneNumber.Insert(0, "0");
                    phoneNumber = phoneNumber.Insert(3, "-");

                    foreach (var s in myStudents) //get the new key for upcoming student
                    {
                        keyTrackerForPhone = s.StudentId + 1;
                        break;
                    }
                    if (breakLoop == true)
                    {
                        break;
                    }
                }
                return phoneNumber;
            }

            string getEmail()
            {
                Console.Clear();
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
                        Console.WriteLine("Personnummer: " + personNumber);
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
                return email;
            }

            int getCourseAndClass()
            {
                //// get the new upcoming studentId and saves it in "keytracker"
                string chooseCourse = "";

                var myClass = from classes in context.Classes select classes;
                myClass = myClass.OrderByDescending(c => c.ClassId);
                var myCourse = from Course in context.Courses select Course;

                foreach (var classData in myClass)
                {
                    keyTracker = classData.FkStudentId + 1; // new students fk_studentId in "Class"
                    break;
                }
                // get courseInformation
                while (true)
                {
                    bool breakLoop = false;
                    Console.Clear();
                    Console.WriteLine("--------------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (var course in myCourse)
                    {
                        Console.WriteLine("#" + course.CourseId + ": " + course.CourseName);
                    }
                    Console.ResetColor();

                    Console.WriteLine("--------------------");
                    Console.Write("\nMata in siffra för kursen eleven ska gå på: ");
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


                // due to course, classroom will automatic generates
                switch (chooseCourse)
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
                getCourse = Convert.ToInt32(chooseCourse);

                return getCourse;
            }

            void addStudentToDatabase()
            {
                if (addEmail == "y")
                {
                    context.Students.Add(new Student(personNumber, firstname, lastname, gender, email)); // if student has a email
                }
                else
                {
                    context.Students.Add(new Student(personNumber, firstname, lastname, gender)); // student has no email, email = NULL
                }
                context.SaveChanges();

                context.Classes.Add(new Class(keyTracker, getCourse, getClassroomId));
                context.SaveChanges();

                context.Phones.Add(new Phone() { FkStudentId = keyTrackerForPhone, PhoneNumber = phoneNumber });
                context.SaveChanges();

                Console.WriteLine("Eleven har lagts till");
                System.Threading.Thread.Sleep(1500);
            }
        }

        private void addEmployee()
        {
            string firstname = getFirstname();
            string lastname = getLastname();
            string gender = getGender();
            int titleId = getTitle();
            int salaryId = getSalary();


            // ---Local funktion---
            string getFirstname()
            {
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
                return firstname;
            }

            string getLastname()
            {
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
                return lastname;
            }

            string getGender()
            {
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
                return gender;
            }

            int getTitle()
            {
                while (true)
                {
                    var title = from t in context.Titles select t;
                    bool breakLoop = false;
                    Console.Clear();
                    while (true)
                    {
                        foreach (var titles in title)
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
                    foreach (var checktitles in title)
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
                return titleId;
            }

            int getSalary()
            {
                while (true)
                {
                    var ShowSalaryStages = from m in context.Salaries select m;
                    bool breakLoop = false;
                    Console.Clear();
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
                return salaryId;
            }

            context.Employees.Add(new Employee(firstname, lastname, gender, titleId, salaryId));
            context.SaveChanges();
            Console.WriteLine("Medarbetare har lagts till");
        }

        private static void ReadSqlTable(string sqlQuery)
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

        private void CountAllEMployee() // #9: Hur många anställda jobbar på de olika avdelningarna "); // EF i VS"
        {
            string getData = "";
            string getTitle = "";
            int count = 0;
            string prevData = ""; // collecting previously data and compare the previously data to the current data
            bool skipRowInFirstLoop = true;
            var employee = from emp in context.Employees.OrderBy(emp => emp.FkTitleId)
                           join title in context.Titles on emp.FkTitleId equals title.TitleId
                           select new
                           {
                               getTitle = title.TitelName,
                               getData = "Employee-ID " + emp.EmployeeId + ": " + emp.EmpFirstName + " " + emp.EmpLastname

                           };
            Console.Clear();
            foreach (var emps in employee)
            {
                if (emps.getTitle != prevData && skipRowInFirstLoop == false) // entering foreach for the first time, this will be skipped
                {
                    Console.WriteLine("Antal anställda: " + count);
                }
                if (prevData == emps.getTitle)
                {
                    count++;
                }
                else
                {
                    skipRowInFirstLoop = false;
                    Console.WriteLine("-------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(emps.getTitle);
                    Console.ResetColor();
                    count = 1;
                }
                prevData = emps.getTitle;
            }
            Console.WriteLine("Antal anställda: " + count);
            Console.ReadKey();
        }

        private void allDataInStudent()
        {
            string phone;
            string name;
            string prevStudentData = ""; // collecting previously data for controlling the foreach-loop for prevent duplicate data
            var student = from s in context.Students.OrderBy(s => s.StudentId)
                          join ph in context.Phones on s.StudentId equals ph.FkStudentId
                          select new
                          {
                              phone = ph.PhoneNumber,
                              name = "ID: " + s.StudentId + "\n" +
                                     s.StudPersonNumber + "\n" +
                                     s.StudFirstName + " " + s.StudLastName + ", " + s.StudGender + "\n" +
                                     s.StudEmail
                          };
            Console.Clear();
            foreach (var stud in student)
            {
                if (prevStudentData != stud.name) // if the collected data is not the same as the new data, print new data ELSE print all phonenumber
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-------------------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(stud.name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Phone number:");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(stud.phone);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(stud.phone);
                }
                Console.ResetColor();
                prevStudentData = stud.name;
            }
            Console.ReadKey();
        }

        private void displayCourseStatus()
        {
            string userInput;
            var courses = from c in context.Courses select c;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("#1: Hämta alla aktiva kurser");
                Console.WriteLine("#2: Hämta alla Inaktiva kurser");
                Console.Write("Välj alternativ: ");
                userInput = Console.ReadLine();
                userInput = userInput.Trim();
                Console.Clear();

                if (userInput == "1")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Aktiva kurser:");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (var course in courses)
                    {
                        if (course.CourseStatus == "Active")
                        {
                            Console.WriteLine(course.CourseName + ": " + course.CoursePoints + " Poäng");
                        }
                    }
                    break;
                }
                else if (userInput == "2")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Inaktiva kurser:");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (var course in courses)
                    {
                        if (course.CourseStatus == "Inactive")
                        {
                            Console.WriteLine(course.CourseName + ": " + course.CoursePoints + " Poäng");
                        }
                    }
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ogiltigt alternativ, försök igen");
                    System.Threading.Thread.Sleep(1500);
                }
            }
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
