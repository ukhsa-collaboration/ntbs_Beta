#!/bin/bash

set -e
trap 'read -p "Press enter to continue..."' EXIT

ntbs_dir="ntbs_Beta"
migration_dir="ntbs-data-migration"
reporting_dir="ntbs-reporting"
specimen_matching_dir="ntbs-specimen-matching"

read -p "Which branch would you like to deploy? (default live): " BRANCH_NAME
BRANCH_NAME=${BRANCH_NAME:-live}

read -p "Are you running this script on the PHE dev box? (default yes): " ADD_SSH_KEY
ADD_SSH_KEY=${ADD_SSH_KEY:-yes}

pull_repository() {
    echo -e "\nChecking out $1...\n"
    cd "./$1"
    if [[ $ADD_SSH_KEY = "yes" ]]
    then
        git config --add core.sshCommand "ssh -i /E/Scripts/ssh/id_ed25519 -o IdentitiesOnly=yes -o StrictHostKeyChecking=no -p 443"
    fi
    git reset --hard
    git checkout $BRANCH_NAME
    git pull
    cd ..
    echo -e "\nSuccessfully reset $1 to $BRANCH_NAME branch.\n"
}

pull_repository $ntbs_dir
pull_repository $migration_dir
pull_repository $reporting_dir
pull_repository $specimen_matching_dir
