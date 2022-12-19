using Microsoft.EntityFrameworkCore;
using SkolPortalen.Data;
using SkolPortalen.Models;

namespace SkolPortalen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            School school = new School();
            school.Menu();
        }
    }
}

