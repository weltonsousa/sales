using System;

namespace SalesWeb.Services.Exception
{
    public class IntegrityException : ApplicationException
    {

        //Class de relacao de intregidade do relacionamento.
        public IntegrityException(string message) : base(message)
        {

        }
    }
}
