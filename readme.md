### AWS Cloudformation Template Demo

-- Validate the template
```
aws cloudformation validate-template --template-body "file:/Users/hola/Documents/myworks/learn-aws/Net7Demo/deployment.yaml"
```

-- Deploy the template
```
aws cloudformation deploy --template "file:/Users/hola/Documents/myworks/learn-aws/Net7Demo/deployment.yaml" --stack-name dev-cf-stack --parameter-overrides 
```