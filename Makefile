build:
	dotnet build ./src/WebApi/Formicarius.WebApi.csproj --configuration Release

docker:
	docker build -t gajdaltd/formicarius:latest -f ./src/Dockerfile ./src

watch:
	dotnet watch --project ./src/WebApi/Formicarius.WebApi.csproj run

run:
	dotnet run --project ./src/WebApi/Formicarius.WebApi.csproj --configuration Release

test:
	dotnet test ./tests/UnitTests/Formicarius.UnitTests.csproj --configuration Release