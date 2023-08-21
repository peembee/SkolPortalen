using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SkolPortalen.Models;

namespace SkolPortalen.Data
{
    public partial class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Examan> Examen { get; set; } = null!;
        public virtual DbSet<Phone> Phones { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Title> Titles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassId).HasColumnName("Class_ID");

                entity.Property(e => e.FkClassroomId).HasColumnName("FK_ClassroomID");

                entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseID");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");

                entity.HasOne(d => d.FkClassroom)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.FkClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Classroom");

                entity.HasOne(d => d.FkCourse)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.FkCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Course");

                entity.HasOne(d => d.FkStudent)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.FkStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Student");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom");

                entity.Property(e => e.ClassroomId).HasColumnName("Classroom_ID");

                entity.Property(e => e.ClassroomName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClassroomNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId).HasColumnName("Course_ID");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CourseStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FkEmployeeId).HasColumnName("FK_EmployeeID");

                entity.HasOne(d => d.FkEmployee)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.FkEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Employee1");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.EmpFirstName).HasMaxLength(50);

                entity.Property(e => e.EmpGender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmpLastname).HasMaxLength(50);

                entity.Property(e => e.EmploymentDate).HasColumnType("date");

                entity.Property(e => e.FkSalaryId).HasColumnName("FK_SalaryID");

                entity.Property(e => e.FkTitleId).HasColumnName("FK_TitleID");

                entity.HasOne(d => d.FkSalary)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.FkSalaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Salary");

                entity.HasOne(d => d.FkTitle)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.FkTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Title");
            });

            modelBuilder.Entity<Examan>(entity =>
            {
                entity.HasKey(e => e.ExamenId)
                    .HasName("PK__Examen__77119565D8E91DF1");

                entity.Property(e => e.ExamenId).HasColumnName("Examen_ID");

                entity.Property(e => e.ExamenDate).HasColumnType("date");

                entity.Property(e => e.ExamenGrade).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseID");

                entity.Property(e => e.FkEmployeeId).HasColumnName("FK_EmployeeID");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");

                entity.HasOne(d => d.FkCourse)
                    .WithMany(p => p.Examen)
                    .HasForeignKey(d => d.FkCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Examen_Course");

                entity.HasOne(d => d.FkEmployee)
                    .WithMany(p => p.Examen)
                    .HasForeignKey(d => d.FkEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Examen_Employee");

                entity.HasOne(d => d.FkStudent)
                    .WithMany(p => p.Examen)
                    .HasForeignKey(d => d.FkStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Examen_Student1");
            });

            modelBuilder.Entity<Phone>(entity =>
            {
                entity.ToTable("Phone");

                entity.Property(e => e.PhoneId).HasColumnName("Phone_ID");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkStudent)
                    .WithMany(p => p.Phones)
                    .HasForeignKey(d => d.FkStudentId)
                    .HasConstraintName("FK_Phone_Student");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.ToTable("Salary");

                entity.Property(e => e.SalaryId).HasColumnName("Salary_ID");

                entity.Property(e => e.SalaryStages)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.HasIndex(e => e.StudPersonNumber, "UQ__Student__32B8F1B5B6B438E6")
                    .IsUnique();

                entity.Property(e => e.StudentId).HasColumnName("Student_ID");

                entity.Property(e => e.StudEmail).HasMaxLength(50);

                entity.Property(e => e.StudFirstName).HasMaxLength(50);

                entity.Property(e => e.StudGender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StudLastName).HasMaxLength(50);

                entity.Property(e => e.StudPersonNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.ToTable("Title");

                entity.Property(e => e.TitleId).HasColumnName("Title_ID");

                entity.Property(e => e.TitelName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
