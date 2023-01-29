using System;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Amazon.CDK.AWS.APIGateway;
using System.Collections.Generic;

namespace CdkDeploy;
public class CdkDeployStack : Stack
{
    internal CdkDeployStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var stage = id.Split('-')[1];
        var getProductLambda = new Function(this, $"GetProduct-{stage}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/GetProduct/bin/Release/net6.0/"),
            Handler = "GetProduct::GetProduct.Function::FunctionHandler",
            FunctionName = $"GetProduct-{stage}",
            Description = $"Deployed on {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });

        var addProductLambda = new Function(this, $"AddProduct-{stage}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/AddProduct/bin/Release/net6.0/"),
            Handler = "AddProduct::AddProduct.Function::FunctionHandler",
            FunctionName = $"AddProduct-{stage}",
            Description = $"Deployed on {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });

        var updateProductLambda = new Function(this, $"UpdateProduct-{stage}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/UpdateProduct/bin/Release/net6.0/"),
            Handler = "UpdateProduct::UpdateProduct.Function::FunctionHandler",
            FunctionName = $"UpdateProduct-{stage}",
            Description = $"Deployed on {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });
        var deleteProductLambda = new Function(this, $"DeleteProduct-{stage}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/DeleteProduct/bin/Release/net6.0/"),
            Handler = "DeleteProduct::DeleteProduct.Function::FunctionHandler",
            FunctionName = $"DeleteProduct-{stage}",
            Description = $"Deployed on {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });
        var rootOperationLambda = new Function(this, $"RootOperation-{stage}", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/RootOperation/bin/Release/net6.0/"),
            Handler = "RootOperation::RootOperation.Function::FunctionHandler",
            FunctionName = $"RootOperation-{stage}",
            Description = $"Deployed on {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        });

        var api = new RestApi(this, "ProductApi", new RestApiProps
        {
            RestApiName = "Product Service",
            Description = "This service serves products.",
            DeployOptions = new StageOptions
            {
                StageName = stage,
            }
        });

        Model responseModel = api.AddModel("ResponseModel", new ModelOptions
        {
            ContentType = "application/json",
            ModelName = "ResponseModel",
            Schema = new JsonSchema
            {
                Schema = JsonSchemaVersion.DRAFT4,
                Title = "pollResponse",
                Type = JsonSchemaType.OBJECT
            }
        });

        Model errorResponseModel = api.AddModel("ErrorResponseModel", new ModelOptions
        {
            ContentType = "application/json",
            ModelName = "ErrorResponseModel",
            Schema = new JsonSchema
            {
                Schema = JsonSchemaVersion.DRAFT4,
                Title = "errorResponse",
                Type = JsonSchemaType.OBJECT,
            }
        });

        var methodOptions = new MethodOptions
        {
            MethodResponses = new[]
            {
                new MethodResponse
                {
                    StatusCode = "200",
                    ResponseParameters = new Dictionary<string, bool>
                    {
                        { "method.response.header.Content-Type", true },
                        { "method.response.header.Access-Control-Allow-Origin", true },
                        { "method.response.header.Access-Control-Allow-Credentials", true }
                    },
                    ResponseModels = new Dictionary<string, IModel>
                    {
                        { "application/json", responseModel }
                    }
                },
                new MethodResponse
                {
                    StatusCode = "400",
                    ResponseParameters = new Dictionary<string, bool>
                    {
                        { "method.response.header.Content-Type", true },
                        { "method.response.header.Access-Control-Allow-Origin", true },
                        { "method.response.header.Access-Control-Allow-Credentials", true }
                    },
                    ResponseModels = new Dictionary<string, IModel>
                    {
                        { "application/json", errorResponseModel }
                    }
                },
                new MethodResponse
                {
                    StatusCode= "500",
                    ResponseParameters = new Dictionary<string, bool>
                    {
                        { "method.response.header.Content-Type", true },
                        { "method.response.header.Access-Control-Allow-Origin", true },
                        { "method.response.header.Access-Control-Allow-Credentials", true }
                    },
                    ResponseModels = new Dictionary<string, IModel>
                    {
                        { "application/json", errorResponseModel }
                    }
                }
            }
        };

        var products = api.Root.AddResource("products");
        products.AddCorsPreflight(new CorsOptions
        {
            AllowOrigins = Cors.ALL_ORIGINS,
            AllowMethods = Cors.ALL_METHODS
        });

        ILambdaIntegrationOptions options = new LambdaIntegrationOptions
        {
            Proxy = true,
            IntegrationResponses = new[]
            {
                new IntegrationResponse
                {
                    StatusCode = "200",
                    ResponseParameters = new Dictionary<string, string>
                    {
                        { "method.response.header.Content-Type", "'application/json'" },
                        { "method.response.header.Access-Control-Allow-Origin", "'*'" },
                        { "method.response.header.Access-Control-Allow-Credentials", "'true'" }
                    }
                },
                new IntegrationResponse
                {
                    StatusCode = "400",
                    ResponseParameters = new Dictionary<string, string>
                    {
                        { "method.response.header.Content-Type", "'application/json'" },
                        { "method.response.header.Access-Control-Allow-Origin", "'*'" },
                        { "method.response.header.Access-Control-Allow-Credentials", "'true'" }
                    }
                }
            }
        };

        var getProductIntegration = new LambdaIntegration(getProductLambda, options);
        products.AddMethod("GET", getProductIntegration, methodOptions);

        var addProductIntegration = new LambdaIntegration(addProductLambda, options);
        products.AddMethod("POST", addProductIntegration, methodOptions);

        var updateProductIntegration = new LambdaIntegration(updateProductLambda, options);
        products.AddMethod("PUT", updateProductIntegration, methodOptions);

        var deleteProductIntegration = new LambdaIntegration(deleteProductLambda, options);
        products.AddMethod("DELETE", deleteProductIntegration, methodOptions);

        var root = api.Root.AddResource("root");
        var rootOperation = new LambdaIntegration(rootOperationLambda, options);
        root.AddMethod("GET", rootOperation, methodOptions);
    }
}
