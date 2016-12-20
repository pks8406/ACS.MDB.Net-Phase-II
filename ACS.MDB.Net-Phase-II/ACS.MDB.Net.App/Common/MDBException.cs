using System;

namespace ACS.MDB.Net.App.Common
{
    /// <summary>
    /// This class used to handel custom exceptions
    /// </summary>
    public class MDBException : SystemException
    {
        protected MDBException()
        {
        }

        protected MDBException(string errorMessage)
            : base(errorMessage)
        {
        }

        protected MDBException(Exception ex)
            : base(ex.Message, ex)
        {
        }
    }
}