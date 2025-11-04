using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sql;


namespace voting_system
{
    

        public class Constituency : Database
        {
           
            public string Constituencyname;
            public string District;
            public string State;
            public List<Candidate> Candidates;

            
            public Constituency(string name, string district, string state)
            {
                Constituencyname = name;
                District = district;
                State = state;
                Candidates = new List<Candidate>();
            }

            public override void Fetchfromtable() { }
            public override void Updatetotable() { }
            public override void Deletefromtable() { }
            public override void Savetotable() { }

            public void Addcandidate(Candidate candidate) { }
            public void Removecandidate(string candidateName) { }
            public void Display() { }
            public void topcandidate() { }
            public void Tvotes() { }
        }
    
}
