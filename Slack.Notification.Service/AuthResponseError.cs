using System.Collections.Generic;
using System.Diagnostics;

namespace Slack.Notification.Service
{
    [DebuggerDisplay("ErrorCode={ErrorCode}, Description={Description}, SolutionMessage={SolutionMessage}, IsWarning={IsWarning}")]
    public class AuthResponseError
    {
        public string ErrorCode { get; set; }

        public string Description { get; set; }

        public string SolutionMessage { get; set; }

        public bool IsWarning { get; set; }

        //REFACTOR THIS INTO DIFFERENT UTILITY
        public static IEnumerable<AuthResponseError> GetErrorsAndWarnings
        {
            get
            {
                yield return new AuthResponseError
                {
                    ErrorCode = "token_revoked",
                    Description = "Taken has been revoked.",
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "not_authed",
                    Description = "No authentication token provided.",
                    SolutionMessage = "Re-enter your credentials."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_auth",
                    Description = "Invalid authentication token."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "account_inactive",
                    Description = "Authentication token is for a deleted user or team."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_arg_name",
                    Description = "The method was passed an argument whose name falls outside the bounds of common decency. This includes very long names and names with non-alphanumeric characters other than _. If you get this error, it is typically an indication that you have made a very malformed API call."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_array_arg",
                    Description = "The method was passed a PHP-style array argument (e.g. with a name like foo[7]). These are never valid with the Slack API."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_charset",
                    Description = "The method was called via a POST request, but the charset specified in the Content-Type header was invalid. Valid charset names are: utf-8 iso-8859-1."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_form_data",
                    Description = "The method was called via a POST request with Content-Type application/x-www-form-urlencoded or multipart/form-data, but the form data was either missing or syntactically invalid."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "invalid_post_type",
                    Description = "The method was called via a POST request, but the specified Content-Type was invalid. Valid types are: application/json application/x-www-form-urlencoded multipart/form-data text/plain."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "missing_post_type",
                    Description = "The method was called via a POST request and included a data payload, but the request did not include a Content-Type header."
                };

                yield return new AuthResponseError
                {
                    ErrorCode = "request_timeout",
                    Description = "The method was called via a POST request, but the POST data was either missing or truncated."
                };

                yield return new AuthResponseError
                {
                    IsWarning = true,
                    ErrorCode = "missing_charset",
                    Description = "The method was called via a POST request, and recommended practice for the specified Content-Type is to include a charset parameter. However, no charset was present. Specifically, non-form-data content types (e.g. text/plain) are the ones for which charset is recommended."
                };

                yield return new AuthResponseError
                {
                    IsWarning = true,
                    ErrorCode = "superfluous_charset",
                    Description = "The method was called via a POST request, and the specified Content-Type is not defined to understand the charset parameter. However, charset was in fact present. Specifically, form-data content types (e.g. multipart/form-data) are the ones for which charset is superfluous."
                };
            }
        } 
    }
}