FROM centos:7

RUN yum install -y epel-release && yum install -y nodejs npm libunwind file && yum clean all && rm -rf /var/cache/yum
COPY ./publish/linux-x64/ /app/
VOLUME /app/images/
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

CMD [ "/app/Memery" ]
