
#https://dev.to/wolnikmarcin/run-asp-net-core-3-on-kubernetes-with-helm-1o01
#https://github.com/marcredhat/aspnet-core-helm-sample
#https://github.com/marcredhat/workshop/tree/master/.NETCore

FROM registry.access.redhat.com/ubi8/dotnet-31 as build
WORKDIR /app

# copy csproj and restore
COPY app/*.csproj ./aspnetapp/
RUN cd ./aspnetapp/ && dotnet restore && dotnet tool install --global dotnet-ef 

# copy all files and build
COPY app/. ./aspnetapp/
WORKDIR /app/aspnetapp

RUN dotnet add package Microsoft.AspNetCore.Http.Abstractions  && dotnet add package TimeSpan2 && dotnet add package prometheus-net && dotnet add package  prometheus-net.AspNetCore && dotnet restore && dotnet publish -f netcoreapp3.1 -c Release -o out


FROM registry.access.redhat.com/ubi8/dotnet-31-runtime  as runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENV ASPNETCORE_URLS="http://*:8080"
EXPOSE 8080
ENTRYPOINT [ "dotnet", "app.dll" ]
