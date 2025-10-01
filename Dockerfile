FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copy all application files from the current directory
# (they will be in the root of the build context)
COPY ./*.dll ./
COPY ./*.json ./
COPY ./*.pdb ./
COPY ./MakerSchedule.API ./
COPY ./runtimes ./runtimes/
COPY ./EmailTemplates ./EmailTemplates/

# Create wwwroot directory
RUN mkdir -p wwwroot

# Expose port
EXPOSE 5000

# Set the entry point
ENTRYPOINT ["dotnet", "MakerSchedule.API.dll", "--urls", "http://0.0.0.0:5000"]
