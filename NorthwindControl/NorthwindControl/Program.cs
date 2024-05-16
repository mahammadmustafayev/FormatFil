using NorthwindControl.Models;
using System.Data.SqlClient;

namespace NorthwindControl;

public class Program
{
    private const string connectionstring = "server=localhost;database=Northwind;uid=sa;pwd=247Pro!!;TrustServerCertificate=true;";
    static void Main(string[] args)
    {


        var employees = GetAllEmployees();

        foreach (var employee in employees)
        {
            Console.WriteLine($"Id: {employee.EmployeeId} , {employee.Title},{employee.TitleOfCourtesy} {employee.FirstName} {employee.LastName},{employee.Country}-{employee.City}");
            Console.WriteLine(new string('-', 70));

        }

        //var employees = GetEmployee(3);

        //foreach (var employee in employees)
        //{
        //    Console.WriteLine($"""
        //        Title:{employee.Title}
        //        FullName:{employee.TitleOfCourtesy} {employee.FirstName} {employee.LastName}
        //        Country:{employee.Country}
        //        City:  {employee.City}
        //        """);


        //}

        //Employee fehle = new Employee();
        //fehle.FirstName = "Eli";
        //fehle.LastName = "Alizade";
        //fehle.Title = "Fehleleriyik";
        //fehle.TitleOfCourtesy = "Mellim";
        //fehle.City = "Baki";
        //fehle.Country = "Azerbaycan";

        //bool result = Add(fehle);
        //Console.WriteLine(result ? "Added successfully" : "Invalid proces");

        //Employee fehle = new Employee();
        //fehle.FirstName = "Ramal";
        //fehle.LastName = "Abbasov";
        //fehle.Title = "CEO";
        //fehle.TitleOfCourtesy = "Jr.";
        //fehle.City = "Babek";
        //fehle.Country = "Azerbaijan";

        //bool result = Edit(1011, fehle);
        //Console.WriteLine(result ? "Edited successfully" : "Invalid proces");

        //bool result = Delete(1010);
        //Console.WriteLine(result ? "Deleted successfully" : "Invalid proces");
    }
    // DELETE FROM Employees WHERE EmployeeID={id};
    public static List<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();
        using (SqlConnection connection = new(connectionstring))
        {
            using (SqlCommand command = new(cmdText: "select EmployeeID,FirstName,LastName,Title,TitleOfCourtesy,City,Country from Employees",
                                            connection: connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new();
                        foreach (var property in typeof(Employee).GetProperties())
                        {
                            property.SetValue(employee, reader[property.Name]);
                        }
                        employees.Add(employee);
                    }
                }
            }
        }
        return employees;
    }

    public static List<Employee> GetEmployee(int id)
    {
        var employees = new List<Employee>();
        using (SqlConnection connection = new(connectionstring))
        {
            using (SqlCommand command = new(cmdText: $"SELECT EmployeeID,FirstName,LastName,Title,TitleOfCourtesy,City,Country FROM Employees Where  EmployeeID={id}",
                                            connection: connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new();
                        foreach (var property in typeof(Employee).GetProperties())
                        {
                            property.SetValue(employee, reader[property.Name]);
                        }
                        employees.Add(employee);
                    }
                }
            }
        }
        return employees;
    }

    public static bool Delete(int id)
    {
        using (SqlConnection connection = new(connectionString: connectionstring))
        {
            using (SqlCommand command = new(cmdText: $"DELETE FROM Employees WHERE EmployeeID={id}", connection: connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", id);
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }

        }
    }

    public static bool Add(Employee employee)
    {
        using (SqlConnection connection = new(connectionstring))
        {
            string commandtext = $"""
                INSERT INTO Employees (FirstName,LastName,Title,TitleOfCourtesy,City,Country)
                VALUES (@FirstName,@LastName,@Title,@TitleOfCourtesy,@City,@Country);
                """;
            using (SqlCommand command = new(cmdText: commandtext, connection: connection))
            {
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Title", employee.Title);
                command.Parameters.AddWithValue("@TitleOfCourtesy", employee.TitleOfCourtesy);
                command.Parameters.AddWithValue("@City", employee.City);
                command.Parameters.AddWithValue("@Country", employee.Country);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }



    public static bool Edit(int id, Employee employee)
    {
        using (SqlConnection connection = new(connectionString: connectionstring))
        {
            using (SqlCommand command = new(cmdText: $"""
                UPDATE Employees
                SET FirstName = @FirstName, LastName = @LastName,Title=@Title,TitleOfCourtesy=@TitleOfCourtesy,City=@City,Country=@Country
                WHERE EmployeeId = {id};
                """, connection: connection))
            {
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Title", employee.Title);
                command.Parameters.AddWithValue("@TitleOfCourtesy", employee.TitleOfCourtesy);
                command.Parameters.AddWithValue("@City", employee.City);
                command.Parameters.AddWithValue("@Country", employee.Country);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }



}