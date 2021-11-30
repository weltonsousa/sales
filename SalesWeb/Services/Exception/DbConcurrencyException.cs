using System;


namespace SalesWeb.Services.Exception
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException(string messages) : base(messages)
        {

        }

    }
}
