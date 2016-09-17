using System;

using Amazon.Runtime;
using Amazon.EC2;

public class AwsException : Exception
{
    // Campos correspondentes da AmazonEC2Exception
    public readonly string ErrorCode;
    public readonly new string Message;
    public readonly string StatusCode;
    public readonly string Type;
    public readonly string RequestId;

    // Mensagem inteligível.
    public readonly string Explanation;

    public AwsException(AmazonEC2Exception ex)
    {
        ErrorCode = ex.ErrorCode;
        Message = ex.Message;
        StatusCode = ex.StatusCode.ToString();
        Type = ex.ErrorType.ToString();
        RequestId = ex.RequestId;
        Explanation = GetExplanation(ex.ErrorType);
    }

    #region Public Overriding
    public override string ToString()
    {
        string result = base.ToString();
        result += string.Format(
            "Exception {0}: {1}\n{2}\nRequest: {3} - Status: {4}\n{5}",
            ErrorCode, Type, Message, RequestId, StatusCode, Explanation
        );

        return result;
    }
    #endregion Public Overriding

    #region Mensagens inteligíveis (em português).
    private string GetExplanation(ErrorType exceptionType)
    {
        switch (exceptionType.ToString())
        {
            case "AuthFailure":
                return "A conta utilizada não está registrada no Amazon AWS ou não possui as devidas permissões.";
            default: return "";
        }
    }
    #endregion Mensagens inteligíveis.
}
