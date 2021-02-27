FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
WORKDIR /app

# Copy entire solution
RUN echo "Copying entire solution..."
COPY . ./

# # install NodeJS 13.x
# # see https://github.com/nodesource/distributions/blob/master/README.md#deb
RUN echo "Installing NodeJS..."
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs

# Restore
RUN echo "Restoring dependencies..."
RUN dotnet restore

# Publish
RUN echo "Building solution..."
RUN dotnet publish -c Release -o out

# Build runtime image
RUN echo "Building runtime image..."
FROM mcr.microsoft.com/dotnet/sdk:5.0
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "Tipper.dll" ]
