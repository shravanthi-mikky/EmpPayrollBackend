using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OfficeOpenXml;
using RepositoryLayer.Interface;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;


namespace RepositoryLayer.Services
{
    public class EmployRL : IEmployRL
    {
        private IConfiguration config;
        //SqlConnection sqlConnection1;
        NpgsqlConnection sqlConnection;
        //string ConString = "Data Source=LAPTOP-2UH1FDRP\\MSSQLSERVER01;Initial Catalog=EmpAngularjs;Integrated Security=True;";
        string ConnString = "Server=localhost;Port=5432;Database=EmpAngularjs;Username=postgres; Password=Mickey@27;Integrated Security=True;";
        public EmployRL(IConfiguration config)
        {
            this.config = config;
        }

        public EmployModel AddEmploy(EmployModel emp)
        {
            try
            {
                using (sqlConnection = new NpgsqlConnection(ConnString))
                {
                    NpgsqlCommand com = new NpgsqlCommand("call Sp_AddEmployee(:Name,:Profile,:Gender,:Department,:Salary,:StartDate)", sqlConnection);
                    com.CommandType = System.Data.CommandType.Text;
                    sqlConnection.Open();
                    com.Parameters.AddWithValue("Name",DbType.String).Value= emp.Name;
                    com.Parameters.AddWithValue("Profile", DbType.String).Value = emp.Profile;
                    com.Parameters.AddWithValue("Gender", DbType.String).Value = emp.Gender;
                    com.Parameters.AddWithValue("Department", DbType.String).Value = emp.Department;
                    com.Parameters.AddWithValue("Salary", DbType.String).Value = emp.Salary;
                   // com.Parameters.AddWithValue("@UserId", Id);
                    com.Parameters.AddWithValue("StartDate", DbType.String).Value = emp.StartDate;

                    com.ExecuteNonQuery();
                    return emp;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public List<EmployModel> GetAllEmployees()
        {
            List<EmployModel> employ = new List<EmployModel>();
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            using (conn)
            {
                try
                {
                    
                    string query = "select * from EmployDetail;";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    conn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employ.Add(new EmployModel
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = reader["Name"].ToString(),
                                Profile = reader["Profile"].ToString(),
                                Gender = reader["Gender"].ToString(),
                                Department = reader["Department"].ToString(),
                                Salary = reader["Salary"].ToString(),
                                StartDate = reader["StartDate"].ToString()
                            });
                        }
                        return employ;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public EmployModel RetriveEmploy(long EmployeeId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            string query = "select * from EmployDetail where EmployeeId= '" + EmployeeId + "';";
            NpgsqlCommand com = new NpgsqlCommand(query, conn);
            com.CommandType = CommandType.Text;
            conn.Open();
            EmployModel employ = new EmployModel();
            NpgsqlDataReader reader = com.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employ.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    employ.Name = reader["Name"].ToString();
                    employ.Profile = reader["Profile"].ToString();
                    employ.Gender = reader["Gender"].ToString();
                    employ.Department = reader["Department"].ToString();
                    employ.Salary = reader["Salary"].ToString();
                    employ.StartDate = reader["StartDate"].ToString();
                }
                return employ;
            }
            return null;
        }
        /*
        public object ExportAsPdf()
        {
            List<EmployModel> employ = new List<EmployModel>();
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            using (conn)
            {
                try
                {

                    string query = "select * from EmployDetail;";
                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    conn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employ.Add(new EmployModel
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                Name = reader["Name"].ToString(),
                                Profile = reader["Profile"].ToString(),
                                Gender = reader["Gender"].ToString(),
                                Department = reader["Department"].ToString(),
                                Salary = reader["Salary"].ToString(),
                                StartDate = reader["StartDate"].ToString()
                            });
                        }
                        //return employ;

                        PdfDocument doc = new PdfDocument();
                        PdfPage page = doc.Pages.Add();
                        PdfGrid pdfGrid = new PdfGrid();

                        IEnumerable<object> dataTable = employ;
                        pdfGrid.DataSource = dataTable;
                        pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
                        MemoryStream stream = new MemoryStream();
                        doc.Save(stream);
                        //If the position is not set to '0' then the PDF will be empty.
                        stream.Position = 0;
                        //Close the document.
                        doc.Close(true);
                        //Defining the ContentType for pdf file.
                        string contentType = "application/pdf";
                        //Define the file name.
                        string fileName = "Output.pdf";

                        FileStream fileStream = new FileStream("Output.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
                        //Save and close the PDF document 
                        doc.Save(fileStream);
                        doc.Close(true);
                       return FileInfo(stream, contentType, fileName);
                    }
                    else { return false; }
                }
                catch (Exception ex)   { throw ex; }
                finally { conn.Close(); }
                return false;
            }
        }
        */

        public bool DeleteEmploy(int EmployID)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            string query = "Delete from EmployDetail where EmployeeId='" + EmployID + "';";

            NpgsqlCommand com = new NpgsqlCommand(query, conn);
            com.CommandType = CommandType.Text;

            conn.Open();
            int i = com.ExecuteNonQuery();
            conn.Close();
            if (i != null)
            {
                return true;
            }
            return false;
        }

        public bool exportData()
        {
            sqlConnection = new NpgsqlConnection(ConnString);
            using (sqlConnection)
                try
                {
                    DateTime dateTime = DateTime.Now;
                    string date = dateTime.ToString("dd_MM_yyyy hh_mm");
                    var file = new FileInfo(@"C:/Users/Admin/Desktop/WebPractice/EmpPayrollBackend/EmployPayroll/Book1.xlsx");
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (ExcelPackage excel = new ExcelPackage(file))
                    {
                        ExcelWorksheet sheet = excel.Workbook.Worksheets["Sheet1"];
                        sqlConnection.Open();
                        NpgsqlCommand command = new NpgsqlCommand("select * from EmployDetail", sqlConnection);
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        int count = dataTable.Rows.Count;
                        sheet.Cells.LoadFromDataTable(dataTable, true);
                        FileInfo excelFile = new FileInfo(@"C:/Users/Admin/Desktop/WebPractice/EmpPayrollBackend/EmployPayroll/Result/" + date + ".xlsx");
                        excel.SaveAs(excelFile);
                        sqlConnection.Close();
                        if (count > 0)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
        }

        public EmployModel UpdateEmploy(EmployModel employ)
        {
            try {
                NpgsqlConnection conn = new NpgsqlConnection(ConnString);
                using (conn)
                {
                    string query = "Update EmployDetail set Name = '" + employ.Name + "',Profile = '" + employ.Profile + "', Gender = '" + employ.Gender + "',Department = '" + employ.Department + "',Salary = '" + employ.Salary + "',StartDate = '" + employ.StartDate + "' where EmployeeId = '" + employ.EmployeeId + "';";
                    NpgsqlCommand com = new NpgsqlCommand(query, conn);
                    
                    conn.Open();
                    int i = com.ExecuteNonQuery();
                    conn.Close();
                    if (i != null)
                    {
                        return employ;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
