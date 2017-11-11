# DotNetBlog
blog.guorj.cn

at `Package Manager Console`
```
More than one DbContext was found. Specify which one to use. 
Use the '-Context' parameter for PowerShell commands and the '--context' parameter for dotnet commands.

PM> add-migration -Context GuorjBlogDbContext Init

PM> update-database --context=GuorjBlogDbContext
```

RID(Runtime Identifier) https://github.com/dotnet/corefx/blob/master/pkg/Microsoft.NETCore.Platforms/runtime.json
Deploying .NET Core apps with command-line interface (CLI) tools
https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="10.0.2" />
</ItemGroup>
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r osx.10.11-x64
Windows RIDs
https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#Using RIDs