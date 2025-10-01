#!/bin/bash
# SSL Certificate Renewal Script
# Run this script periodically (e.g., via cron) to renew SSL certificates

cd /tmp/makerschedule-deploy

# Attempt to renew certificate
docker compose run --rm certbot renew

# Reload nginx if certificate was renewed
if [ $? -eq 0 ]; then
    echo "Certificate renewed successfully. Reloading nginx..."
    docker exec makerschedule_nginx nginx -s reload
else
    echo "Certificate renewal failed or not needed yet."
fi
