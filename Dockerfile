FROM microsoft/dotnet
MAINTAINER dpritchett@gmail.com

ADD . /opt/devchatterbot

WORKDIR /opt/devchatterbot

CMD ["cd /opt/devchatterbot/src/UnitTests", "dotnet test"]
