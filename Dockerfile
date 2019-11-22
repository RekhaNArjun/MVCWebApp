FROM microsoft/aspnet
WORKDIR /inetpub/wwwroot
COPY ./Publish .
