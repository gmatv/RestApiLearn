using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RestApiLearn.Exceptions
{
    public class ErrorInfo
    {
        public ErrorInfo(string message)
        {
            ErrorMessages = new[] { new ErrorMessage(message) };
        }

        public ErrorInfo(ErrorMessage[] errorMessages)
        {
            ErrorMessages = errorMessages;
        }
        public ErrorMessage[] ErrorMessages { get; }

        public override string ToString()
        {
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }
    }
}
