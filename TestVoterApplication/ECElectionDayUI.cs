using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{
    internal class ECElectionDayUI
    {
        TimeOnly votingtime_open = new TimeOnly(8);
        TimeOnly votingtime_close = new TimeOnly(18);

        static string connection_string = "server = LAPTOP-MDVNPCGM;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        ResidentVoter resident_voter = new ResidentVoter();
        NRIVoter nri_voter = new NRIVoter();
        

        public void ConnectToDatabase()
        {
            conn_voter.Open();
        }
        public void DisconnectToDatabase()
        {
            conn_voter.Close();
        }
        // // On Election Day: If the current date and time matches the election day and is after 8 am till 6 pm then we can 
        // // open this to show the voters the voting side.
        // // Otherwise thsi wont open.
        // // Before opening we will set the voter's voting rights to be 0, which wont be called again later.
        // // The voters will get the options to upload their name and constituency, which will help in fetching their voter details
        

        public void ResetVotingRightsInDB()
        {
            try
            {
                ConnectToDatabase();
                var sqlQuerry = "Select * from VOTER_TABLE";
                using (SqlCommand command = new SqlCommand(sqlQuerry, conn_voter))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                resident_voter.ResetVotingStatusinDB(reader.GetString(0));
                            }
                        }
                        throw new ArgumentException("NO ENTRIES FOUND IN TABLE");
                    }
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            finally
            {
                DisconnectToDatabase();
            }

            
        }

        public int GetVoterDetailsOnPrompt()
        {
            try
            {
                Console.WriteLine("Please let us know wether you are");
                Console.WriteLine("[1] A Resident Voter");
                Console.WriteLine("[2] A NRI Voter");
                Console.WriteLine("[3] I want to exit the Application!");

                var user_type = Convert.ToInt16(Console.ReadLine());

                switch (user_type)
                {
                    case 1:
                        resident_voter.ViewVoterDetails();
                        Console.WriteLine("Are the Details Correct? :");
                        Console.Write("Yes [1] or No [2]");
                        var user_detail_verif_resident = Convert.ToInt16(Console.ReadLine());
                        if (user_detail_verif_resident == 1)
                        {
                            Console.WriteLine("Thank you for confirming!");
                            return user_type;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please Enter your details again!");
                            GetVoterDetailsOnPrompt();
                            break;
                        }
                    case 2:
                        nri_voter.ViewVoterDetails();
                        Console.WriteLine("Are the Details Correct? :");
                        Console.Write("Yes [1] or No [2]");
                        var user_detail_verif_nri = Convert.ToInt16(Console.ReadLine());
                        if (user_detail_verif_nri == 1)
                        {
                            Console.WriteLine("Thank you for confirming!");
                            return user_type;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please Enter your details again!");
                            GetVoterDetailsOnPrompt();
                            break;
                        }
                    case 3:
                        Console.WriteLine("Thank You for coming to vote! We hope we could change your mind!");
                        throw new ArgumentException("USER_DENIED_VOTE");
                        
                        break;

                    default:
                        Console.WriteLine("Please Select A Valid Option Next Time!");
                        GetVoterDetailsOnPrompt();
                        break;


                }
                return -1;
            }
            catch(Exception E)
            {
                Console.WriteLine(E.ToString());
                return -1;
            }
        }
        // // Once voter details are confirmed, we will send them to the next stage that is the voting section
        // // In the voting Section we will be showing all the candidates and have the voter select from the candidates that they want
        // // All methods are implemented already
        // // Once Candidate selection is done we will ensure that a new entry has been added to the table, make the voter status as 1 
        // // Then we will again show the entrance page

        public void VotingScreenUI()
        {
            var user_type = GetVoterDetailsOnPrompt();
            if (user_type > 0)
            {
                switch (user_type)
                {
                    case 1:
                        resident_voter.CastVote();
                        Console.WriteLine("Thank You For Casting Your Vote! Every Vote Matters!");
                        break;
                    case 2:
                        nri_voter.CastVote();
                        Console.WriteLine("Thank You For Casting Your Vote! Every Vote Matters!");
                        Console.WriteLine("Ever THougt About Visiting India Once Again?");
                        Console.WriteLine("See The results of your voting in the developement of India!");
                        Console.WriteLine("Contact India Tourism For More Details!");
                        Console.WriteLine("Contact Number: 8999009321");
                        break;
                    default:
                        Console.WriteLine("Please Select the Proper option Again!");
                        VotingScreenUI();
                        break;
                }

                Console.WriteLine("Would You Like To return To the Voting Page or Exit to the main menu?");
                Console.WriteLine("[1] Return to Voting Page");
                Console.WriteLine("[2] Return To Main Menu");
                user_type = Convert.ToInt16(Console.ReadLine());

                if (user_type == 1)
                {
                    VotingScreenUI();
                }
            }
        }

        public void DisplayElectionResults()
        {
            ElectionCommission.DisplayResults();
            Console.WriteLine("The Final Votes Are Counted and the Results are as the following:");
            ElectionCommission.DisplayOverallWinner();
            
        }
        

        // If date time implemented then this can be moved forward with.
        //public void CheckVotingDateTime(string constituency)
        //{
        //    if(Convert.ToInt16(DateTime.Now.Hour)<Convert.ToInt16(votingtime_close) && Convert.ToInt16(DateTime.Now.Hour) > Convert.ToInt16(votingtime_open))
        //    {

        //    }
        //}
    }
}
