# DotNetBlog

www.guorj.cn

RID(Runtime Identifier) 
  * https://github.com/dotnet/corefx/blob/master/pkg/Microsoft.NETCore.Platforms/runtime.json

Deploying .NET Core apps with command-line interface (CLI) tools
  * https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli

```bash
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r osx.10.11-x64
```

```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="10.0.2" />
</ItemGroup>
```

Windows RIDs

  * [https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#Using RIDs](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#Using RIDs)

Run in a Linux container 

 * https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.0

## Bundling and Minification
  * https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-3.0
  * https://github.com/madskristensen/BundlerMinifier
  * https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier
  * https://dotnetthoughts.net/bundling-and-minification-in-aspnet-core/

## Docker
https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile

```Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
RUN dotnet restore

# copy everything else and build app
COPY aspnetapp/. ./aspnetapp/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
```

## MySql

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

---

## Editor.md 

The open source embeddable online markdown editor (component). http://editor.md.ipandao.com/

  * https://github.com/pandao/editor.md
  * http://editor.md.ipandao.com/

## Marked.js 

A markdown parser and compiler. Built for speed. https://marked.js.org/

  * https://github.com/markedjs/marked
  * https://marked.js.org/demo/

  ## MailKit

  A cross-platform .NET library for IMAP, POP3, and SMTP. http://www.mimekit.net
DotNet官方推荐使用MailKit而不是使用自带的：https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient

> We don't recommend that you use the *SmtpClient* class for new development because *SmtpClient* doesn't support many modern protocols. Use [MailKit](https://github.com/jstedfast/MailKit) or other libraries instead. For more information, see [SmtpClient shouldn't be used](https://github.com/dotnet/platform-compat/blob/master/docs/DE0005.md) on GitHub.

    * https://github.com/jstedfast/MailKit