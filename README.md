


dotnet ef migrations add "Modify_Permission" -c TeduIdentityContext -s Tedu.Identity.IDP.csproj -p ../Tedu.Identity.Infrastructure/Tedu.Identity.Infrastructure.csproj -o Persistence/Migrations



Database update

dotnet ef database update -c PersistedGrantDbContext -s src/Tedu.Identity.IDP/Tedu.Identity.IDP.csproj

dotnet ef database update -c ConfigurationDbContext -s src/Tedu.Identity.IDP/Tedu.Identity.IDP.csproj

dotnet ef database update -c TeduIdentityContext -s src/Tedu.Identity.IDP/Tedu.Identity.IDP.csproj -p src/Tedu.Identity.Infrastructure/Tedu.Identity.Infrastructure.csproj