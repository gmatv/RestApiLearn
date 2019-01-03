using Microsoft.EntityFrameworkCore.Internal;

namespace RestApiLearn.Exceptions
{
    public class ErrorMessage
    {
        public ErrorMessage(string messageFormat, params object[] args)
        {
            MessageFormat = messageFormat;
            Args = args;
            if (!string.IsNullOrEmpty(messageFormat) && args.Any())
            {
                Message = string.Format(messageFormat, args);
            }
            else
            {
                Message = messageFormat;
            }
        }
        public ErrorMessage(string[] fields, string messageFormat, params object[] args)
            : this(messageFormat, args)
        {
            Fields = fields;
        }

        public string[] Fields { get; }
        public string MessageFormat { get; }
        public object[] Args { get; }
        public string Message { get; }

    }
}
