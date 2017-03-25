# Animeci REST API

Animeci is a mobile application built with Ionic 2, ASP.NET Core, Entity Framework Core and PostgreSQL that is able to stream anime episodes as well as track progress of the user.

Animeci, anime bölümlerinin izlenebildiği ve takibinin yapılabildiği Ionic 2, ASP.NET Core, Entity Framework Core and PostgreSQL ile geliştirilmiş bir mobil uygulamadır.

## Setup

````sh
sudo apt-get install apt-transport-https ufw
sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt/ `lsb_release -cs`-pgdg main" >> /etc/apt/sources.list.d/pgdg.list'
wget -q https://www.postgresql.org/media/keys/ACCC4CF8.asc -O - | sudo apt-key add -
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893

sudo apt-get install dotnet-dev-1.0.1 postgresql postgresql-contrib

# firewall configuration
ufw enable
ufw allow http
ufw allow https
ufw allow postgresql
ufw allow ssh

#psql settings

nano /etc/postgresql/9.6/main/pg_hba.conf
host     all     all     127.0.0.1/32     trust
host     all     all     0.0.0.0/0     trust

nano /etc/postgresql/9.6/main/postgresql.conf
listen_addresses = "*"

#psql dumping & restoring
pg_dump animeci -U postgres -h localhost -Fc > animeci-psql.bak
pg_restore -e -d animeci -U postgres -h localhost animeci-psql.bak
psql -d animeci -U postgres -h localhost -W


# nginx
# /etc/nginx/sites-available/default
server {
listen 80;
location / {
                proxy_pass http://127.0.0.1:5000/;
                #proxy_pass http://unix:/run/kestrel-animeciapi.sock;
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_cache_bypass $http_upgrade;
        }
}
````