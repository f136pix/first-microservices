FROM --platform=linux/amd64 php:8.1 as php

RUN apt-get update -y
RUN apt-get install -y unzip libpq-dev libcurl4-gnutls-dev
RUN docker-php-ext-install pdo pdo_mysql bcmath sockets curl

WORKDIR /var/www
COPY . .

COPY --from=composer:2.3.5 /usr/bin/composer /usr/bin/composer

#RUN chmod +x Docker/entrypoint.sh
ENV PORT=8000

RUN composer install --no-progress --no-interaction && \
    composer update --no-progress --no-interaction

CMD  php artisan key:generate && \
     php artisan cache:clear && \
     php artisan config:clear && \
     php artisan route:clear && \
     php artisan serve --port=$PORT --host=0.0.0.0 --env=.env
