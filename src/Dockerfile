FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-alpine3.9

WORKDIR /app
COPY DotNetBlog/bin/Release/netcoreapp3.0/publish/ .
EXPOSE 80

ENTRYPOINT ["dotnet", "DotNetBlog.dll"]