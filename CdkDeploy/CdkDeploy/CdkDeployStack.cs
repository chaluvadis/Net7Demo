using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Amazon.CDK.AWS.APIGateway;

namespace CdkDeploy;
public class CdkDeployStack : Stack
{
    internal CdkDeployStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var getProductLambda = new Function(this, "GetProduct-Dev", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/GetProduct/bin/Release/net6.0/publish"),
            Handler = "GetProduct::GetProduct.Function::FunctionHandler",
            FunctionName = "GetProduct-Dev"
        });

        var addProductLambda = new Function(this, "AddProduct-Dev", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/AddProduct/bin/Release/net6.0/publish"),
            Handler = "AddProduct::AddProduct.Function::FunctionHandler",
            FunctionName = "AddProduct-Dev"
        });

        var updateProductLambda = new Function(this, "UpdateProduct-Dev", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/UpdateProduct/bin/Release/net6.0/publish"),
            Handler = "UpdateProduct::UpdateProduct.Function::FunctionHandler",
            FunctionName = "UpdateProduct-Dev"
        });
        var deleteProductLambda = new Function(this, "DeleteProduct-Dev", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/DeleteProduct/bin/Release/net6.0/publish"),
            Handler = "DeleteProduct::DeleteProduct.Function::FunctionHandler",
            FunctionName = "DeleteProduct-Dev"
        });
        var rootOperationLambda = new Function(this, "RootOperation-Dev", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Code = Code.FromAsset("/Users/hola/Documents/myworks/learn-aws/Net7Demo/RootOperation/bin/Release/net6.0/publish"),
            Handler = "RootOperation::RootOperation.Function::FunctionHandler",
            FunctionName = "RootOperation-Dev"
        });

        var api = new RestApi(this, "ProductApi", new RestApiProps
        {
            RestApiName = "Product Service",
            Description = "This service serves products."
        });

        var products = api.Root.AddResource("products");

        var getProductIntegration = new LambdaIntegration(getProductLambda);
        products.AddMethod("GET", getProductIntegration);

        var addProductIntegration = new LambdaIntegration(addProductLambda);
        products.AddMethod("POST", addProductIntegration);

        var updateProductIntegration = new LambdaIntegration(updateProductLambda);
        products.AddMethod("PUT", updateProductIntegration);

        var deleteProductIntegration = new LambdaIntegration(deleteProductLambda);
        products.AddMethod("DELETE", deleteProductIntegration);

        var root = api.Root.AddResource("root");
        var rootOperation = new LambdaIntegration(rootOperationLambda);
        root.AddMethod("GET", rootOperation);
    }
}
