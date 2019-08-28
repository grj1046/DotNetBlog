# DotNetBlog
www.guorj.cn

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


mysql

```
[mysqld]
datadir=C:/Program Files/MariaDB 10.3/data
port=3306
innodb_buffer_pool_size=1007M
character_set_server=utf8
[client]
port=3306
plugin-dir=C:/Program Files/MariaDB 10.3/lib/plugin
```