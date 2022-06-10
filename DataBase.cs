using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel_accounting
{
    public class DataBase
    {
        public SqlConnection Connection { get; private set; }

        public DataBase(string databaseName)
        {
            Connection = new SqlConnection($@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={MainForm.PathToProgram}\{databaseName};Integrated Security=SSPI");
        }

        public List<string> GetRow(string rowName)
        {
            Open();
            var row = new List<string>();
            var query = $@"select {rowName} from personal";
            var command = new SqlCommand(query, Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
                row.Add(reader.GetString(0));
            Close();
            return row;
        }

        public List<string> GetFiltredRow(string rowName, string filterRowName, string filter)
        {
            Open();
            var row = new List<string>();
            if (filter.Length > 0) 
            { 
                var query = $@"select {rowName} from personal where {filterRowName}='{filter}'";
                var command = new SqlCommand(query, Connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                    row.Add(reader.GetString(0));
            }
            Close();
            return row;
        }

        public List<string> GetDistinctedRow(string rowName)
        {
            Open();
            var row = new List<string>();
            var query = $@"select distinct {rowName} from personal";
            var command = new SqlCommand(query, Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
                row.Add(reader.GetString(0));
            Close();
            return row;
        }

        public Person GetInformationAboutPerson(string fullname)
        {
            Open();
            var query = $@"select * from personal where fullname='{fullname}'";
            var command = new SqlCommand(query,Connection);
            var reader = command.ExecuteReader();
            Person person = null;
            while (reader.Read())
            {
                var splitedFullname = reader["fullname"].ToString().Split();
                person = new Person(splitedFullname[0], splitedFullname[1], splitedFullname[2], (string)reader["birthDate"]
                    , reader["sex"].ToString(), reader["profession"].ToString(), reader["division"].ToString()
                    , reader["director"].ToString());
            }
            Close();
            return person;
        }

        public void CreateRecord(Person person)
        {
            Open();
            var query = $@"insert into personal (fullname, birthDate, sex, profession, division, director) values('{person.FullName}', '{person.BirthDate}', '{person.Sex}', '{person.Profession}', '{person.Division}', '{person.Director}')";
            var command = new SqlCommand(query,Connection);
            command.ExecuteNonQuery();
            MainForm.UpdateDataSource();
            Close();
        }

        public void EditRecord(string oldFullname,Person person)
        {
            Open();
            var query = $@"update personal set fullname='{person.FullName}', birthDate='{person.BirthDate}', sex='{person.Sex}', profession='{person.Profession}', division='{person.Division}', director='{person.Director}' where fullname='{oldFullname}'";
            var command = new SqlCommand(query, Connection);
            command.ExecuteNonQuery();
            MainForm.UpdateDataSource();
            Close();
        }

        public void DeleteRecord(string fullname)
        {
            Open();
            var query = $@"delete from personal where fullname='{fullname}'";
            var command = new SqlCommand(query,Connection);
            command.ExecuteNonQuery(); 
            MainForm.UpdateDataSource();
            Close();
        }

        private void Open()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }

        private void Close()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
        }
    }
}
