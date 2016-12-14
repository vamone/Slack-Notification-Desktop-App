using System;
using System.Diagnostics;
using System.Linq;

namespace Slack.Api
{
    [DebuggerDisplay("IsSuccess={IsSuccess}, Message={Message}")]
    [DebuggerDisplay(
        "ErrorCode={AuthResponseError.ErrorCode}, Description={AuthResponseError.Description}, SolutionMessage={AuthResponseError.SolutionMessage}, IsWarning={AuthResponseError.IsWarning}"
        )]
    public class InitializationResults
    {
        private const string DefaultInitializationMessage = "Initialization Successful.";

        public InitializationResults()
        {
            this.Message = DefaultInitializationMessage;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public AuthResponseError AuthResponseError { get; set; }

        internal InitializationResults ValidateInitialization(AuthResponse auth)
        {
            var init = new InitializationResults();

            try
            {
                if (auth == null)
                {
                    throw new ArgumentNullException(nameof(auth));
                }
                
                bool isStatusOk = auth.IsStatusOk;
                if (!isStatusOk)
                {
                    var responseError =
                        AuthErrorsAndWarnings.Get.SingleOrDefault(x => x.ErrorCode.Equals(auth.Error));

                    init.Message = responseError != null
                        ? responseError.Description
                        : auth.Error;

                    init.AuthResponseError = responseError;

                    return init;
                }

                //TODO: FEATURE VALIDATE COMPONENTS

                init.IsSuccess = true;
            }
            catch (Exception ex)
            {
                init.Message = ex.Message;
            }

            return init;
        }
    }
}