using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestVoterApplication
{
    internal class NRIVoter : Voter
    {
        static string connection_string = "server = LAPTOP-MDVNPCGM;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };
        

        public void ConnectToDatabase()
        {
            conn_voter.Open();
        }
        public void DisconnectToDatabase()
        {
            conn_voter.Close();
        }
        public override void SetVoterDetails(string name, DateOnly date_of_birth, string voter_id, string constituency, string polling_booth, int voting_status)
        {
            try
            {ConnectToDatabase();
            string sqlQuery = "INSERT INTO VOTER_TABLE VALUES(@VOTER_ID,@NAME,@DATE_OF_BIRTH,@CONSTITUENCY,@POLLING_BOOTH,@VOTING_STATUS)";

            using(SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
            {
                    //command.Parameters.AddWithValue("@TABLE_NAME", "VOTER_TABLE");
                    command.Parameters.AddWithValue("@VOTER_ID", voter_id);
                    command.Parameters.AddWithValue("@NAME", name);
                    command.Parameters.AddWithValue("@DATE_OF_BIRTH", date_of_birth);
                    command.Parameters.AddWithValue("@CONSTITUENCY", constituency);
                    command.Parameters.AddWithValue("@POLLING_BOOTH", polling_booth);
                    command.Parameters.AddWithValue("@VOTING_STATUS", voting_status);
                    //SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, conn_voter);
                    //DataTable datatable = new DataTable();
                    //adapter.Fill(datatable);

                    int affectedRows = command.ExecuteNonQuery();
                    Console.WriteLine($"Insert successful. Rows affected: {affectedRows}");

                }
                DisconnectToDatabase();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }
        public override string GetVoterId(string name, string constituency)
        {
            string sqlQuerry = "Select * from VOTER_TABLE where NAME=@VOTER_NAME and CONSTITUENCY=@VOTER_CONSTITUENCY";
            
            try
            {
                ConnectToDatabase();
                using (SqlCommand command = new SqlCommand(sqlQuerry, conn_voter))
                {
                    command.Parameters.AddWithValue("@VOTER_NAME", name);
                    command.Parameters.AddWithValue("@VOTER_CONSTITUENCY", constituency);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var voter_id = reader.GetString(0);
                                Console.WriteLine($"VOTER_ID: {voter_id}");
                                DisconnectToDatabase();
                                return voter_id;
                            }
                        }
                        throw new ArgumentException("NO_DATA_FOUND_IN_RESULT");
                    }
                }
                
            }
            catch(Exception e)
            {
                DisconnectToDatabase();
                Console.WriteLine(e.ToString());
                return "NO DATA FOUND ERROR";
            }
        }
        public override string GetVoterName()
        {
            Console.WriteLine("Please Enter Your Name as Mentioned in the Voter List: ");
            var name = Console.ReadLine();
            return name;
        }
        public override int GetVoterStatus(int age, bool isInBlacklist, string voter_id)
        {
            try
            {
                ConnectToDatabase();
                if (age >= 18 && !isInBlacklist)
                {
                    var sqlQuerry = "Select * from VOTER_TABLE where VOTER_ID = @VOTER_ID";
                    using (SqlCommand command = new SqlCommand(sqlQuerry, conn_voter))
                    {
                        command.Parameters.AddWithValue("@VOTER_ID", voter_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var voting_status = reader.GetInt32(5);

                                    if (voting_status == 0)
                                    {
                                        Console.WriteLine($"THE VOTER IS ELIGIBLE TO VOTE");
                                       
                                        return voting_status;
                                    }
                                    else if(voting_status==1)
                                    {
                                        Console.WriteLine("<==========UPDATING_VOTER_DATABASE==========>");
                                        Console.WriteLine("<==========THE_VOTER_HAS_ALREADY_VOTED==========>");
                                        //sqlQuerry = "Update VOTER_TABLE set VOTING_STATUS = 0 where VOTER_ID = @VOTER_ID";
                                        //using (SqlCommand command_update = new SqlCommand(sqlQuerry, conn_voter))
                                        //{
                                        //    command_update.Parameters.AddWithValue("@VOTER_ID", voter_id);
                                        //    Console.WriteLine("<==========UPDATED_VOTER_DATABASE==========>");
                                        //}
                                        Console.WriteLine($"THE VOTER IS ELIGIBLE TO VOTE NEXT YEAR");
                                        
                                        return 1;
                                    }
                                    else
                                    {
                                        Console.WriteLine("<==========UPDATING_VOTER_DATABASE==========>");
                                        Console.WriteLine("<==========THE_VOTER_HAS_BEEN_BANNED==========>");
                                        //sqlQuerry = "Update VOTER_TABLE set VOTING_STATUS = 0 where VOTER_ID = @VOTER_ID";
                                        //using (SqlCommand command_update = new SqlCommand(sqlQuerry, conn_voter))
                                        //{
                                        //    command_update.Parameters.AddWithValue("@VOTER_ID", voter_id);
                                        //    Console.WriteLine("<==========UPDATED_VOTER_DATABASE==========>");
                                        //}
                                        Console.WriteLine($"THE VOTER IS NOT ELIGIBLE TO VOTE NEXT YEAR");
                                        
                                        return voting_status;
                                    }
                                }
                            }
                        }
                    }

                }
                throw new ArgumentException("NO_DATA_FOUND_IN_RESULT");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
                return -2;
            }
            finally
            {
                DisconnectToDatabase();
            }
        }

        public override string GetVoterConstituency()
        {
            Console.WriteLine("Please Enter Your Constituency as Mentioned in the Voter List: ");
            var constituency = Console.ReadLine();
            return constituency;
        }
        public override string GetVoterPollingBooth(string voter_id)
        {
            try
            {
                ConnectToDatabase();
                string sqlQuery = "Select * from VOTER_TABLE where VOTER_ID = @VOTER_ID";
                
                using(SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                    command.Parameters.AddWithValue("@VOTER_ID", voter_id);
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var polling_booth = reader.GetString(4);
                                Console.WriteLine($"POLLING_BOOTH = {polling_booth}");
                                return polling_booth;
                            }
                        }
                        throw new ArgumentException("NO_DATA_FOUND_IN_RESULT");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return "POLLING_BOOTH_GET_ERROR";
            }
            finally
            {
                DisconnectToDatabase();
            }
        }
        public override int GetVoterAge(string voter_id)
        {
            try
            {
                ConnectToDatabase();
                string sqlQuery = "Select * from VOTER_TABLE where VOTER_ID = @VOTER_ID";

                using (SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                    command.Parameters.AddWithValue("@VOTER_ID", voter_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var age = Math.Truncate((DateTime.Now.Date- reader.GetDateTime(2).Date).TotalDays/365);
                                Console.WriteLine($"AGE = {age}");
                                return Convert.ToInt16(age);
                            }
                        }
                        throw new ArgumentException("NO_DATA_FOUND_IN_RESULT");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
            finally
            {
                DisconnectToDatabase();
            }
        }

        public string GetVoterCurrentResidence(string voter_id)
        {
            try
            {
                ConnectToDatabase();
                var sqlQuery = "Select * from PASSPORT_VERIFICATION where VOTER_ID=@voter_id";
                using(SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                    command.Parameters.AddWithValue("@voter_id", voter_id);
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(2);
                            }
                        }
                        throw new ArgumentException("The Passport holder lives in an undisclosed country");
                    }
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E.ToString());
                return "Passport Holder Not Found...ERROR";
            }
            finally
            {
                DisconnectToDatabase();
            }

        }
        public string GetVoterCurrentPassportID()
        {
            Console.WriteLine("<==========NRI_PASSPORT_ENTRY==========>");
            Console.Write("PLEAE ENTER YOUR PASSPORT NUMBER: ");
            var passport_num = Console.ReadLine();
            Console.WriteLine("<==========THANK_YOU_FOR_YOUR_COOPERATION==========>");

            return passport_num;
        }
        public string GetVoterCurrentPassportID(string voter_id)
        {
            try
            {
                ConnectToDatabase();
                var sqlQuery = "Select * from PASSPORT_VERIFICATION where VOTER_ID=@voter_id";
                using (SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                    command.Parameters.AddWithValue("@voter_id", voter_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                        }
                        throw new ArgumentException("The Passport Number is not documented");
                    }
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.ToString());
                return "Passport Holder Not Found...ERROR";
            }
            finally
            {
                DisconnectToDatabase();
            }
        }

        public void RegisterVoteInDB(string name, int age, string constituency, string candidate, string voter_id, bool isInBlacklist) {

            if (age >= 18 && VerifyPassportUser(voter_id))
            {
                try
                {
                    var voting_status = GetVoterStatus(age, isInBlacklist, voter_id);
                    ConnectToDatabase();

                    if(voting_status==0)
                    {
                        var sqlQuerry = "Insert into VOTING_DAY_TABLE values (@CONSTITUENCY, @CANDIDATE_NAME)";
                        using (SqlCommand command = new SqlCommand(sqlQuerry, conn_voter))
                        {
                            command.Parameters.AddWithValue("@CONSTITUENCY", constituency);
                            command.Parameters.AddWithValue("@CANDIDATE_NAME", candidate);
                            command.ExecuteNonQuery();
                            Console.WriteLine("Thank You For Voting!");
                            sqlQuerry = "Update VOTER_TABLE set VOTING_STATUS = 1 where VOTER_ID = @VOTER_ID";
                            using (SqlCommand command_update = new SqlCommand(sqlQuerry, conn_voter))
                            {
                                command_update.Parameters.AddWithValue("@VOTER_ID", voter_id);
                                int affected_rows = command_update.ExecuteNonQuery();
                                Console.WriteLine("<==========UPDATED_VOTER_DATABASE==========>");
                            }
                        }
                    }
                    else
                    {
                        throw new AlreadyVotedException(name);

                    }
                }
                catch(AlreadyVotedException av)
                {
                    Console.WriteLine("<==========ERROR==========>");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    DisconnectToDatabase();
                }
            }
            
        }

        public override void CastVote()
        {
            var name = GetVoterName();
            var constituency = GetVoterConstituency();
            var id = GetVoterId(name,constituency);
            var age = GetVoterAge(id);
            var sqlQuery = "Select * from CANDIDATE_TABLE where CONSTITUENCY=@voter_constituency";
            try
            {
                ConnectToDatabase();
                using (SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                        command.Parameters.AddWithValue("@voter_constituency", constituency);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                int count = 0;
                                Dictionary<int, string> candidate_table = new Dictionary<int, string>();
                                Console.WriteLine("The Candidates Are: ");


                                while (reader.Read())
                                {
                                    Console.WriteLine($"[{count + 1}]::: Name: {reader.GetString(0)}, Constituency: {reader.GetString(1)}, Party: {reader.GetString(2)}, Candidate_ID: {reader.GetString(3)}");
                                    candidate_table.Add(++count, reader.GetString(0));
                                }

                                Console.Write("Please Enter the Candidate Number You would like to vote for [1,2,3...] or Press [0] for NOTA : ");
                                var choice_user = Convert.ToInt16(Console.ReadLine());
                                if (choice_user <= count && count != 0)
                                {
                                    RegisterVoteInDB(name, age, constituency, candidate_table[choice_user], id, false);
                                }
                                else if (choice_user == 0)
                                {
                                    RegisterVoteInDB(name, age, constituency, "__NOTA__", id, false);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Candidate Chosen, Please go throught the list again");
                                    throw new InvalidCandidateException();

                                }
                            }
                        }
                    }
                }
            catch(InvalidCandidateException iv)
            {
                CastVote();
            }
            catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            finally
            {
                DisconnectToDatabase();
            }

            
        }

        bool VerifyPassportUser(string voter_id)
        {
            try
            {
                ConnectToDatabase();
                var passport = GetVoterCurrentPassportID();
                var sqlQuerry = "Select * from PASSPORT_VERIFICATION where VOTER_ID=@voter_id";
                using (SqlCommand command = new SqlCommand(sqlQuerry, conn_voter))
                {
                    command.Parameters.AddWithValue("@voter_id", voter_id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                if (passport == reader.GetString(0))
                                {
                                    Console.WriteLine("<==========PASSPORT_VERIFICATION_COMPLETE==========>");
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("<==========PASSWORD_VERIFICATION_FAILED==========>");
                                    throw new ArgumentException("PASSWORD_VERIFICATION_FAILED_ERROR");
                                    
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Passport Exists...Error");
                            throw new ArgumentException("PASSPORT_DOESNT_EXIST_ERROR");

                        }
                    }
                    Console.WriteLine("No Such Passport exists");
                    throw new ArgumentException("PASSPORT_DOESNT_EXIST_ERROR");

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            finally
            {
                DisconnectToDatabase();
            }
            
        }

        public override void ResetVotingStatusinDB(string voter_id) {

            ConnectToDatabase();
            var sqlQuerry = "Update VOTER_TABLE set VOTING_STATUS = 0 where VOTER_ID = @VOTER_ID";
            using (SqlCommand command_update = new SqlCommand(sqlQuerry, conn_voter))
            {
                command_update.Parameters.AddWithValue("@VOTER_ID", voter_id);
                int affected_rows = command_update.ExecuteNonQuery();
                if (affected_rows > 0)
                {
                    Console.WriteLine("<==========UPDATED_VOTER_DATABASE==========>");
                }
                else
                {
                    Console.WriteLine("<==========NO_RECORDS_FOUND==========>");
                }
            }
            DisconnectToDatabase();
        }
        public override void ViewVoterDetails()
        {
            
            try
            {
                string name = GetVoterName();
                string constituency = GetVoterConstituency();
                string voter_id = GetVoterId(name, constituency);
                string voter_age = Convert.ToString(GetVoterAge(voter_id));
                string voter_passport = GetVoterCurrentPassportID(voter_id);
                string voter_current_residence = GetVoterCurrentResidence(voter_id);

                Console.WriteLine("<==========VOTER_DETAILS_REQUESTED==========>");
                Console.WriteLine($"Name: {name}");
                Console.WriteLine($"Constituency: {constituency}");
                Console.WriteLine($"Age: {voter_age}");
                Console.WriteLine($"Voter_ID: {voter_id}");
                Console.WriteLine($"Passport Num: {voter_passport}");
                Console.WriteLine($"Current Residence: {voter_current_residence}");



            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("<==========FINISHED==========>");
            }
        }

        public override string ToString()
        {
            ViewVoterDetails();
            return "Redundant_Function_call";
        }
        public NRIVoter() { }
    }
}
