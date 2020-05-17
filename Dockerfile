FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /MovieRentalApi/MovieRentalApi/
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MovieRentalAdminApi.dll
