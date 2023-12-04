dotnet ef migrations add "Modify_Permission" -c TeduIdentityContext -s Tedu.Identity.IDP.csproj -p ../Tedu.Identity.Infrastructure/Tedu.Identity.Infrastructure.csproj -o Persistence/Migrations

dotnet ef database update -c TeduIdentityContext -s Tedu.Identity.IDP.csproj -p ../Tedu.Identity.Infrastructure/Tedu.Identity.Infrastructure.csproj