using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimchaDBManager
{
    public class SimchaManager
    {
        private string _connectionstring;
        public SimchaManager(string _Connectionstring)
        {
            _connectionstring = _Connectionstring;
        }
        public IEnumerable<Transaction> GetTransactionsbyidSorted(int id)
        {
            List<Transaction> transactions = GetDepositTransactionsbyid(id);
           // transactions.AddRange(GetContributionTransactionsbyid(id));
            transactions.OrderBy(t => t.Date);
            return transactions;
        }
        private List<Transaction> GetContributionTransactionsbyid(int id)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"select s.Name,sc.Amount from SimchaContribution sc
                                join Simchas s on
                                sc.Simchaid = s.id
                                where sc.Personid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<Transaction> transactions = new List<Transaction>();
            con.Open();
            SqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                transactions.Add(new Transaction
                {
                    Action = $"Contribution to the {(string)Reader["Name"]} simcha",
                    Date = (DateTime)Reader["Date"],
                    Amount = -(decimal)Reader["Amount"]
                });
            }
            con.Close();
            con.Close();
            return transactions;
        }
        private List<Transaction> GetDepositTransactionsbyid(int id)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"Select * from Deposits d
                                where d.Personid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            List<Transaction> transactions = new List<Transaction>();
            con.Open();
            SqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                transactions.Add(new Transaction
                {
                    Action = "Deposit",
                    Date = (DateTime)Reader["Date"],
                    Amount = (decimal)Reader["Amount"]
                });
            }
            con.Close();
            con.Close();
            return transactions;
        }
        public void AddContribution(Contribution C)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"insert into SimchaContribution
                                Values(@Sid,@Pid,@Amount) ";
            cmd.Parameters.AddWithValue("@Sid", C.Simchaid);
            cmd.Parameters.AddWithValue("@Pid", C.Personid);
            cmd.Parameters.AddWithValue("@Amount", C.Amount);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
        public void AddSimcha(Simcha s)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"insert into Simchas
                                Values(@Name, @Date); ";
            cmd.Parameters.AddWithValue("@Name", s.Name);
            cmd.Parameters.AddWithValue("@Date", s.Date);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
        public void AddPerson(Person p)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"insert into People
                                Values(@FN,@LN,@Age,@Cell,@DC); ";
            cmd.Parameters.AddWithValue("@FN", p.FirstName);
            cmd.Parameters.AddWithValue("@LN", p.LastName);
            cmd.Parameters.AddWithValue("@Age", p.Age);
            cmd.Parameters.AddWithValue("@Cell", p.Cell);
            cmd.Parameters.AddWithValue("@DC", p.dContributor);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
        public IEnumerable<Contribution> GetContributionsById(int id)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"select * from SimchaContribution
                                Where Personid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var Conts = new List<Contribution>();
            con.Open();
            SqlDataReader Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                Conts.Add(new Contribution
                {
                    id = (int)Reader["id"],
                    Personid = (int)Reader["Personid"],
                    Simchaid = (int)Reader["Simchaid"],
                    Amount = (decimal)Reader["Amount"]
                });
            }
            con.Close();
            con.Close();
            return Conts;
        }
        public IEnumerable<Simcha> GetSimchas()
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"Select s.id, s.Date, s.Name, ISNULL(SUM(SC.Amount),0) as Collected from SimchaContribution SC
                                right join Simchas s
                                on SC.Simchaid = s.id
                                Group by s.id, s.Date, s.Name";
            List<Simcha> simchas = new List<Simcha>();
            con.Open();
            SqlDataReader Reader = cmd.ExecuteReader();
            while(Reader.Read())
            {

                Simcha s = new Simcha
                {
                    Id = (int)Reader["id"],
                    Date = (DateTime)Reader["Date"],
                    Name = (string)Reader["Name"],
                    MRaised = (decimal)Reader["Collected"]
                };
                s.TotalContributors = GetContributorsCount(s.Id);
                simchas.Add(s);
            }
            con.Close();
            con.Close();
            return simchas;
        }
        public IEnumerable<Person> GetPeople()
        {
            List<int> PeopleIds = GetPeopleIds();
            List<Person> people = new List<Person>();
            PeopleIds.ForEach(p =>
            {
                people.Add(GetPersonById(p));
            });
            return people;
        }
        public Person GetPersonById(int id)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"exec RetrievePersonWithBalanceById @id = @Personid";
            cmd.Parameters.AddWithValue("@Personid", id);
            con.Open();
            Person person = new Person();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            person.id = (int)reader["id"];
            person.FirstName = (string)reader["FirstName"];
            person.LastName = (string)reader["LastName"];
            person.Age = (int)reader["Age"];
            person.Cell = (string)reader["Cell"];
            person.dContributor = (bool)reader["dContributor"];
            person.Balance = (decimal)reader["Balance"];
            con.Close();
            con.Dispose();
            return person;
        }
        private List<int> GetPeopleIds()
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select p.id from People p";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<int> count = new List<int>();
            while (reader.Read())
            { 
                count.Add((int)reader["id"]);
            }
            con.Close();
            con.Dispose();
            return count;
        }
        public int GetPeopleCount()
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select Count(p.id) from People p";
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            con.Dispose();
            return count;
        }
        public int GetContributorsCount(int simchaid)
        {
            SqlConnection con = new SqlConnection(_connectionstring);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"Select ISNULL(Count(Distinct sc.Personid),0) from SimchaContribution sc
                                Where sc.Simchaid = @id";
            cmd.Parameters.AddWithValue("@id", simchaid);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();
            con.Dispose();
            return int.Parse(count.ToString());
        }
    }
}
