using Amazon.CDK;

namespace CdkDeploy;
sealed class Program
{
    public static void Main()
    {
        var app = new App();
        IStackProps stackProps = new StackProps
        {
            Description = "This is a CDK stack for deploying .NET 7 Lambda functions to AWS.",
        };
        _ = new CdkDeployStack(app, "CdkDeployStack-dev", stackProps);
        app.Synth();
    }
}
