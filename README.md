# DotNetBlog
blog.guorj.cn

at `Package Manager Console`
```
More than one DbContext was found. Specify which one to use. 
Use the '-Context' parameter for PowerShell commands and the '--context' parameter for dotnet commands.

PM> add-migration -Context GuorjBlogDbContext Init

PM> update-database --context=GuorjBlogDbContext
```