#!/bin/bash
# Check if the SQL file already exists
if [[ -f ./database/mysql/02-user_privileges.sql ]]; then
    echo "User privileges SQL file already exists. Skipping generation."
    exit 0
fi

echo "Generating user privileges SQL file..."

# Ensure environment variables are set
if [[ -z "$MYSQL_DATABASE" || -z "$MYSQL_USER" || -z "$MYSQL_PASSWORD" ]]; then
    echo "Error: Required environment variables MYSQL_DATABASE, MYSQL_USER, or MYSQL_PASSWORD are not set."
    exit 1
fi

# Write SQL content directly with embedded variables
cat > ./database/mysql/02-user_privileges.sql <<EOL
-- user_privileges.sql for initializing roles and users

-- Define roles
CREATE ROLE IF NOT EXISTS 'app_role';
CREATE ROLE IF NOT EXISTS 'admin_role';
CREATE ROLE IF NOT EXISTS 'readonly_role';
CREATE ROLE IF NOT EXISTS 'restricted_role';

-- Grant specific privileges to each role
GRANT SELECT, INSERT, UPDATE, DELETE ON \`${MYSQL_DATABASE}\`.* TO 'app_role';  -- Application User Role
GRANT ALL PRIVILEGES ON \`${MYSQL_DATABASE}\`.* TO 'admin_role';                -- Full Admin Role
GRANT SELECT ON \`${MYSQL_DATABASE}\`.* TO 'readonly_role';                     -- Read-Only Role
GRANT SELECT ON \`${MYSQL_DATABASE}\`.vessel TO 'restricted_role';      -- Restricted Role with limited access

-- Create users and assign them to roles

-- Application User (minimum privileges)
CREATE USER IF NOT EXISTS 'app_user'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
GRANT 'app_role' TO 'app_user';

-- Full Admin User
CREATE USER IF NOT EXISTS 'admin_user'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
GRANT 'admin_role' TO 'admin_user';

-- Read-Only User
CREATE USER IF NOT EXISTS 'readonly_user'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
GRANT 'readonly_role' TO 'readonly_user';

-- Restricted Read-Only User
CREATE USER IF NOT EXISTS 'restricted_user'@'%' IDENTIFIED BY '${MYSQL_PASSWORD}';
GRANT 'restricted_role' TO 'restricted_user';
EOL

echo "User privileges SQL file generated."
