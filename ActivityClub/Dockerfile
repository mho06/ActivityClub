WORKDIR /src
COPY . .

# Updated path to match where your .csproj actually is
RUN dotnet restore "ActivityClub/ActivityClub.csproj"
RUN dotnet publish "ActivityClub/ActivityClub.csproj" -c Release -o /app/publish
