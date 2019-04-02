using System;

namespace Battleship.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        
    }

    
}
