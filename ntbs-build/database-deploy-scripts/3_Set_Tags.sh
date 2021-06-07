#!/bin/bash

set -e
trap 'read -p "Press enter to continue..."' EXIT

ntbs_dir="ntbs_Beta"
migration_dir="ntbs-data-migration"
reporting_dir="ntbs-reporting"
specimen_matching_dir="ntbs-specimen-matching"

read -p "Enter the release number (e.g. 1.2.3): " RELEASE_NUMBER

if [[ -z "$RELEASE_NUMBER" ]]
then
    echo "You must choose a release number."
    exit 1
fi

read -p "Are you running this script on the PHE dev box? (default yes): " ADD_SSH_KEY
ADD_SSH_KEY=${ADD_SSH_KEY:-yes}

set_tag_in_repository() {
    echo -e "\Adding tag $2 to $1 repository...\n"
    cd "./$1"
    if [[ $ADD_SSH_KEY = "yes" ]]
    then
        git config --add core.sshCommand "ssh -i /E/Scripts/ssh/id_ed25519 -o IdentitiesOnly=yes -o StrictHostKeyChecking=no -p 443"
    fi
    git tag "$2"
    git push origin "$2"
    cd ..
    echo "Successfully added tag $2 to $1 repository."
}

printf -v date_stamp '%(%Y%m%d)T' -1
tag="Release-$RELEASE_NUMBER-$date_stamp"

set_tag_in_repository $ntbs_dir $tag
set_tag_in_repository $migration_dir $tag

set_tag_in_repository $reporting_dir $tag
set_tag_in_repository $specimen_matching_dir $tag
