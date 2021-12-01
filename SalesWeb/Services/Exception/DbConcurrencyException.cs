using System;


namespace SalesWeb.Services.Exception
{
    public class DbConcurrencyException : ApplicationException
    {
        //meu construtor recebe e repassa para class base
        public DbConcurrencyException(string messages) : base(messages)
        {

        }

    }
}
