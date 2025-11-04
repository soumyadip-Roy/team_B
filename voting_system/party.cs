using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voting_system
{
    public class Party : Database
    {

        public string PartyName;
        public string PartySymbol;
        public string LeaderName;
        public List<Candidate> Members;

        
        public Party(string name, string symbol, string leader)
        {
            PartyName = name;
            PartySymbol = symbol;
            LeaderName = leader;
            Members = new List<Candidate>();
        }

        
        public override void Fetchfromtable() { }
        public override void Updatetotable() { }
        public override void Deletefromtable() { }
        public override void Savetotable() { }

      
        public void Addcandidate(Candidate candidate) { }
        public void Removecandidate(string candidateName) { }
        public void Display() { }
        public void Tpartyvotes() { }
        public void winner() { }
    }
}
