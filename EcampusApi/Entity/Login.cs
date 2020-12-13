using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcampusApi.Entity
{
    public enum Reason
    {
        None,
        Pasword,
        ErrorServer,
        OtherProblem,
    }
    public class Login
    {
        public bool IsSuccess { get; set; }
        public Reason ReasonFail { get; set; }
    }
}
