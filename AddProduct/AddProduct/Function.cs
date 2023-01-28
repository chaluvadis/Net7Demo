using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AddProduct;

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return $"AddProduct - {request.Body.ToUpper()}";
    }
}

