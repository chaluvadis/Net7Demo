using Amazon.CDK;

namespace CdkDeploy;
sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        var stackProps = new StackProps
        {
            StackName = "CdkDeployStack",
        };
        _ = new CdkDeployStack(app, "CdkDeployStack", null);
        app.Synth();
    }
}
