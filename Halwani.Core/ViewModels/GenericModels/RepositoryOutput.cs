using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.GenericModels
{
    public class RepositoryOutput
    {
        public RepositoryOutput()
        {
            Code = RepositoryResponseStatus.Ok;
            Success = true;
            ErrorMessages = new string[] { };
        }
        public RepositoryResponseStatus Code { get; set; }
        public string[] ErrorMessages { get; set; }
        public bool Success { get; set; }


        public static RepositoryOutput CreateNotAcceptableResponse(string[] errorMessages)
        {
            return new RepositoryOutput
            {
                Success = false,
                ErrorMessages = errorMessages,
                Code = RepositoryResponseStatus.ValidationError
            };
        }

        public static RepositoryOutput CreateErrorResponse()
        {
            return new RepositoryOutput
            {
                Success = false,
                Code = RepositoryResponseStatus.Error
            };
        }

        public static RepositoryOutput CreateErrorResponse(string error)
        {
            return new RepositoryOutput
            {
                Success = false,
                Code = RepositoryResponseStatus.Error,
                ErrorMessages = new string[] { error }
            };
        }

        public static RepositoryOutput CreateNotAllowedResponse()
        {
            return new RepositoryOutput
            {
                Success = false,
                Code = RepositoryResponseStatus.NotAllowed
            };
        }

        public static RepositoryOutput CreateNotFoundResponse()
        {
            return new RepositoryOutput
            {
                Success = false,
                Code = RepositoryResponseStatus.NotFound
            };
        }

        public static RepositoryOutput CreateSuccessResponse()
        {
            return new RepositoryOutput
            {
                Success = true,
                Code = RepositoryResponseStatus.Ok
            };
        }
    }

    public enum RepositoryResponseStatus
    {
        NotFound = -1,
        Ok = 1,
        Error = -2,
        ValidationError = -3,
        NotAllowed = -4
    }
}
